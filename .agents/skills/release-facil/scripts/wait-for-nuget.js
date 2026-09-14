const { setTimeout: sleep } = require('node:timers/promises');

async function waitForPackage(version, {
  fetchImpl = fetch,
  now = () => performance.now(),
  sleep: pause = sleep,
  timeoutMs = 600_000,
} = {}) {
  if (typeof version !== 'string' || !/^\d+\.\d+\.\d+(?:-[0-9a-z.-]+)?$/i.test(version)) {
    throw new Error('Invalid version. Usage: node wait-for-nuget.js <version>, for example 3.2.0');
  }

  version = version.toLowerCase();
  const url = `https://api.nuget.org/v3-flatcontainer/facil/${version}/facil.${version}.nupkg`;
  const deadline = now() + timeoutMs;
  let lastFailure = 'No response';

  while (now() < deadline) {
    let status;
    try {
      const response = await fetchImpl(url, {
        method: 'HEAD',
        headers: { 'Cache-Control': 'no-cache' },
        signal: AbortSignal.timeout(Math.max(1, Math.ceil(Math.min(15_000, deadline - now())))),
      });
      status = response.status;
      lastFailure = `HTTP ${status}`;
    } catch (error) {
      lastFailure = error.message;
    }

    if (status === 200) return url;
    if (status !== undefined && status !== 404 && status !== 429 && status < 500) {
      throw new Error(`Unexpected HTTP ${status} while checking Facil ${version}`);
    }

    const remaining = deadline - now();
    if (remaining > 0) await pause(Math.min(30_000, remaining));
  }

  throw new Error(`Timed out waiting for Facil ${version} on NuGet. Last check: ${lastFailure}`);
}

module.exports = { waitForPackage };

if (require.main === module) {
  waitForPackage(process.argv[2]).then(
    url => console.log(`NuGet package available: ${url}`),
    error => { console.error(error.message); process.exitCode = 1; },
  );
}
