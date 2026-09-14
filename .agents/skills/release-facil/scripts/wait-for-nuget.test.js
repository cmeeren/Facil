const { test } = require('node:test');
const assert = require('node:assert/strict');
const { spawnSync } = require('node:child_process');
const { join } = require('node:path');
const { waitForPackage } = require('./wait-for-nuget.js');

function scenario(outcomes, timeoutMs = 600_000) {
  let elapsed = 0;
  const requests = [];
  const sleeps = [];
  return {
    requests,
    sleeps,
    options: {
      timeoutMs,
      now: () => elapsed,
      sleep: async ms => { sleeps.push(ms); elapsed += ms; },
      fetchImpl: async (url, options) => {
        requests.push({ url, method: options.method });
        const outcome = outcomes[Math.min(requests.length - 1, outcomes.length - 1)];
        if (outcome instanceof Error) throw outcome;
        return { status: outcome };
      },
    },
  };
}

test('accepts HTTP 200 for the exact normalized version', async () => {
  const run = scenario([200]);
  const url = await waitForPackage('3.2.0-RC.1', run.options);
  assert.deepEqual({ url, requests: run.requests, sleeps: run.sleeps }, {
    url: 'https://api.nuget.org/v3-flatcontainer/facil/3.2.0-rc.1/facil.3.2.0-rc.1.nupkg',
    requests: [{ url: 'https://api.nuget.org/v3-flatcontainer/facil/3.2.0-rc.1/facil.3.2.0-rc.1.nupkg', method: 'HEAD' }],
    sleeps: [],
  });
});

test('waits through delayed package availability', async () => {
  const run = scenario([404, 404, 200]);
  await waitForPackage('3.2.0', run.options);
  assert.deepEqual({ calls: run.requests.length, sleeps: run.sleeps }, { calls: 3, sleeps: [30_000, 30_000] });
});

test('retries transient network, server, and rate-limit failures', async () => {
  const run = scenario([new Error('offline'), 503, 429, 200]);
  await waitForPackage('3.2.0', run.options);
  assert.deepEqual({ calls: run.requests.length, sleeps: run.sleeps }, { calls: 4, sleeps: [30_000, 30_000, 30_000] });
});

test('times out without sleeping or requesting beyond the deadline', async () => {
  const run = scenario([404], 45_000);
  const error = await waitForPackage('3.2.0', run.options).catch(error => error.message);
  assert.deepEqual({ timedOut: /Timed out.*HTTP 404/.test(error), calls: run.requests.length, sleeps: run.sleeps }, {
    timedOut: true, calls: 2, sleeps: [30_000, 15_000],
  });
});

test('fails immediately on unexpected HTTP responses', async () => {
  const run = scenario([403]);
  const error = await waitForPackage('3.2.0', run.options).catch(error => error.message);
  assert.deepEqual({ error, calls: run.requests.length, sleeps: run.sleeps }, {
    error: 'Unexpected HTTP 403 while checking Facil 3.2.0', calls: 1, sleeps: [],
  });
});

test('rejects invalid versions before any request', async () => {
  const run = scenario([200]);
  const error = await waitForPackage('../3.2.0', run.options).catch(error => error.message);
  assert.deepEqual({ invalid: /Invalid version/.test(error), calls: run.requests.length }, { invalid: true, calls: 0 });
});

test('CLI failures return nonzero without reporting availability', () => {
  const result = spawnSync(process.execPath, [join(__dirname, 'wait-for-nuget.js'), 'invalid'], { encoding: 'utf8' });
  assert.deepEqual({ status: result.status, stdout: result.stdout, invalid: /Invalid version/.test(result.stderr) }, {
    status: 1, stdout: '', invalid: true,
  });
});
