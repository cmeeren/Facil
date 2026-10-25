---
name: release-facil
description: Prepare and publish Facil releases. Use when updating release notes or package versions, creating release commits and tags, packing release artifacts, pushing release tags, watching tag CI/NuGet publication, or creating GitHub Releases.
---

# Release Facil

## Overview

Use this skill to release Facil through the repo's normal tag-driven NuGet flow, then create a GitHub Release after the tag CI has successfully pushed the package to NuGet. Keep the release change small and user-facing: version fields, release notes, README, `facil_reference.yaml` and consumer skill updates when behavior or configuration changed, verification, commit, tag, push, CI/NuGet confirmation, and GitHub Release publication. Follow the user's existing authorization boundaries for commits, pushes, and publication; preparation alone does not authorize them.

## Workflow

1. Require a clean working directory before starting a release. If `git status --short` reports changes unrelated to the authorized release work, stop and ask how to resolve them. Do not discard or stash user changes automatically. This precondition does not block an explicitly requested documentation edit or continuation of release preparation already underway.

2. Confirm the release shape before editing:
   - Bump `Version` in `Directory.Build.props`.
   - Keep `PackageReleaseNotes` in `src/Facil.Package/Facil.Package.fsproj` pointing at the versioned GitHub Release URL:

````xml
<PackageReleaseNotes>https://github.com/cmeeren/Facil/releases/tag/v/$(Version)</PackageReleaseNotes>
````

   - Use `v/<Version>` for the release tag. The CI workflow publishes packages only for tags under `refs/tags/v/`.

3. Prepare the release content:
   - Update code and docs only as needed for the release.
   - Check `.agents/skills/facil/SKILL.md` against the release's behavior and configuration, including its stated version scope and documentation links, and keep the README's skill installation guidance current.
   - Update `RELEASE_NOTES.md` with user-facing behavior, migration impact, and usage guidance. Add entries under `### Unreleased`, creating that section if necessary.
   - Put actionable breaking changes first, using a `#### Breaking changes` or `#### Upgrading from <previous major>.x` subsection. Explain what affected users need to change, such as regenerating code, changing YAML settings, or updating calls to generated APIs. Group correctness fixes that restore expected behavior; do not catalog niche edge cases unless they have meaningful migration consequences.
   - Use other categorizing subsections only when grouping materially improves readability; for small releases, prefer plain bullets under the version heading.
   - Within each list, order entries by user impact and breadth. Put documentation/package-metadata-only entries last unless they are the main release purpose.
   - Keep `README.md`, `RELEASE_NOTES.md`, and `facil_reference.yaml` public and user-facing. Keep implementation-only rationale out of them. Update `facil_reference.yaml` when public YAML configuration behavior, options, defaults, examples, or reference text change.
   - Before tagging, rename `### Unreleased` to `### <Version> (<YYYY-MM-DD>)`, using the release date.
   - Inspect the versioned notes before committing and tagging. Include only intended public notes, with no stale `Unreleased` heading or private details.
   - Do not create or reuse the GitHub Release notes temp file yet; refresh it from the final tagged content after successful NuGet publication.

4. Verify locally. Mirror CI, using the same release-critical environment variables and the Expecto console apps directly:

````powershell
$env:FACIL_FORCE_REGENERATE = "1"
$env:FACIL_FAIL_ON_CHANGED_OUTPUT = "1"
$env:connectionString = (Get-Content src/DbTests.DbGen/appsettings.json -Raw | ConvertFrom-Json).connectionString

dotnet tool restore
dotnet fantomas --check .
node --test .agents/skills/release-facil/scripts/wait-for-nuget.test.js
dotnet build src/TestDb/TestDb.sqlproj -c Release
dotnet sqlpackage /Action:Publish /SourceFile:"src/TestDb/bin/Release/TestDb.dacpac" /TargetConnectionString:"$env:connectionString"
dotnet build src/Facil.Package -c Release
dotnet pack src/Facil.Package -c Release
dotnet build src/PackageTests -c Release
dotnet src/PackageTests/bin/Release/net10.0/PackageTests.dll --fail-on-focused-tests
dotnet build src/DbTests -c Release
dotnet src/DbTests/bin/Release/net10.0/DbTests.dll --fail-on-focused-tests
````

   Run the block in one shell session so the environment variables apply to every command, and require each command to succeed before continuing. Use the SDK selected by `global.json`. The connection string is the local test database's, which test generation and the package tests also read from `src/DbTests.DbGen/appsettings.json`. If that SQL Server is unavailable, stop and report which CI-equivalent checks could not be run instead of replacing them with weaker `dotnet test` runs. Local success does not replace the tag's CI results.

   The `DbTests` build regenerates the test project, which updates the Facil version recorded in the generated files in `src/DbTests.DbGen`. Include those changes in the release commit.

5. Inspect `nupkg/Facil.<Version>.nupkg`, the package that step 4 packed and smoke-tested: confirm the version, versioned release-notes URL, packaged README and icon, the runtime assemblies under `lib/netstandard2.0/`, the build targets under `build/` and `tools/`, and the generator under the `tools/` framework folder that `FacilGenerate.targets` runs. Check that README links work outside GitHub, since NuGet shows the packaged README. Local pack proves that the release URL is embedded; it cannot resolve until the matching GitHub Release exists. If preparing a major release, inspect important known consumers for affected contracts and run representative compatibility checks when practical, such as regenerating and building a consumer against the packed package. For such checks, repack with `dotnet pack src/Facil.Package -c Release -p:Version=<Version>-compat.<n>`, using an `<n>` not yet in the global packages folder for each repack, rather than restoring step 4's package: NuGet never replaces a cached version, so the release version cached from `nupkg/` would shadow the published package on this machine, and a reused prerelease version would serve the earlier build.

6. Commit and tag when authorized:
   - Use `git-commit` to execute the authorized commit; it routes message authoring to `git-commit-message`.
   - Use `v/<Version>` for releases, for example `v/3.2.0`. Confirm the tag points at the intended release commit and matches the package version.

7. Push the reviewed commit and tag after the user explicitly confirms that action. Successful tag CI publishes the package to NuGet, so pushing the tag is publication authorization, not merely a backup operation.

8. Watch the tag CI run and confirm NuGet publication before creating a GitHub Release:
   - Find the run for the pushed `v/<Version>` tag, not just the branch push run. Verify its `headSha` matches the release commit.

````powershell
$tag = "v/<Version>"
$repo = "cmeeren/Facil"
$sha = git rev-parse "$tag^{commit}"
$run = @(gh run list --repo $repo --workflow ci.yml --branch $tag --event push --limit 10 --json databaseId,headBranch,headSha,status,conclusion,url | ConvertFrom-Json | Where-Object headSha -eq $sha)
if (-not $run) { throw "No CI run found for tag $tag at $sha" }
$runId = $run[0].databaseId
gh run watch $runId --repo $repo --exit-status
````

   - Confirm the tag run succeeded, including its `publish` job. That job pushes only the package whose version matches the tag and fails if NuGet already has that version, so success means this run uploaded it. If it failed, stop and inspect/report before creating a GitHub Release.
   - Verify the exact package version is visible on NuGet with [scripts/wait-for-nuget.js](scripts/wait-for-nuget.js). Run from the repository root with Node.js 22 or newer. The helper checks every 30 seconds for up to ten minutes, bounds each request to 15 seconds, and exits nonzero on timeout or an unexpected HTTP response. It retries HTTP 404, 429, server errors, and network failures; it does not publish anything. Wait using bounded tool waits rather than starting additional polling commands.

````powershell
node .agents/skills/release-facil/scripts/wait-for-nuget.js "<Version>"
if ($LASTEXITCODE -ne 0) { throw "NuGet availability check failed; do not create the GitHub Release" }
````

9. Refresh and inspect the GitHub Release notes immediately before creating the release:
   - Copy the exact release section from the tagged `RELEASE_NOTES.md` to a file under the user's temp directory, for example `$env:TEMP\facil-release-v<Version>.md`.
   - Read the tagged file with `git show "v/<Version>:RELEASE_NOTES.md"` so the notes match the commit that was packaged and published.
   - Inspect the temporary file. Include only the released version's notes, not the whole changelog or a stale `Unreleased` heading. Check that its links work from the GitHub Release page, which resolves relative links such as `README.md#<anchor>` against the release tag.

10. Create the GitHub Release only after successful NuGet publication, notes inspection, and authorization for that action:

````powershell
$tag = "v/<Version>"
$repo = "cmeeren/Facil"
if (gh release view $tag --repo $repo 2>$null) { throw "GitHub Release already exists for $tag" }
gh release create $tag --repo $repo --verify-tag --title "Facil <Version>" --notes-file "$env:TEMP\facil-release-v<Version>.md"
````

   If the existing-release check finds a release, stop instead of creating a duplicate. If the check fails for authentication or connectivity reasons, resolve that uncertainty before creating one. For a prerelease version, also pass `--prerelease`.

11. Verify the created release and remove the temporary notes file when practical:

````powershell
$tag = "v/<Version>"
$repo = "cmeeren/Facil"
gh release view $tag --repo $repo --json tagName,name,url
````

   Confirm the release URL targets the same tag as the `PackageReleaseNotes` URL embedded in the package metadata.

## Guardrails

- Do not create the GitHub Release before the tag CI has successfully pushed to NuGet and the exact version is available.
- Use `gh release create --verify-tag` so GitHub does not auto-create or retarget the release tag.
- Use `--repo cmeeren/Facil` for `gh` release and run commands; do not rely on implicit repository context.
- Do not create duplicate releases. Do not move a release tag once its CI's `Push` step has started, even if it failed, since NuGet never reuses a version. If the tag CI fails earlier, rerun the failed jobs when the failure is transient; if it needs a fix, ask whether to move the tag to the fixed commit, repeating steps 4–8 for it, or release a new version.
- Refresh release notes from tagged content after NuGet publication; do not reuse an older temp file.
- Do not treat the versioned `PackageReleaseNotes` URL as fully verified until the matching GitHub Release exists.
