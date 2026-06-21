---
name: release-facil
description: Prepare and publish Facil releases. Use when updating release notes or package versions, creating release commits and tags, packing release artifacts, pushing release tags, watching tag CI/NuGet publication, or creating GitHub Releases.
---

# Release Facil

## Overview

Use this skill to release Facil through the repo's normal tag-driven NuGet flow, then create a GitHub Release after the tag CI has successfully pushed the package to NuGet. Keep the release change small and user-facing: version fields, release notes, README and `facil_reference.yaml` updates when behavior or configuration changed, verification, commit, tag, push, CI/NuGet confirmation, and GitHub Release publication.

## Workflow

1. Require a clean working directory before proceeding. If `git status --short` reports any changes, stop and ask the user whether to commit, stash, discard, or otherwise resolve them first.

2. Confirm the release shape before editing:
   - Bump `Version` in `Directory.Build.props`.
   - Keep `PackageReleaseNotes` in `src/Facil.Package/Facil.Package.fsproj` pointing at the versioned GitHub Release URL:

````xml
<PackageReleaseNotes>https://github.com/cmeeren/Facil/releases/tag/v/$(Version)</PackageReleaseNotes>
````
   - Use `v/<Version>` for the release tag. The CI workflow publishes packages only for tags under `refs/tags/v/`.

3. Prepare the release content:
   - Update code and docs only as needed for the release.
   - Update `RELEASE_NOTES.md` with user-facing behavior, migration impact, and usage guidance.
   - For user-relevant changes, add a concise entry in the `### Unreleased` section, creating that section if it does not exist.
   - When the release includes any breaking change, create or keep a `#### Breaking` subsection and place it before other categorizing subsections. Put every breaking release-note entry there; do not bury breaking changes under `#### Changed`, `#### Fixed`, or plain bullets.
   - Use categorizing subsections such as `#### Added` or `#### Fixed` only when the release notes are large enough that grouping materially improves readability; for small releases, prefer plain bullets under the version heading.
   - Within each release-note bullet list, whether under the version heading or a categorizing subsection, order entries by user impact and breadth first. Put broad runtime correctness and behavior changes before narrow edge cases, and put documentation/package-metadata-only entries last unless they are the main release purpose.
   - Describe the observable effect from the user's perspective. Avoid naming internal mechanisms or implying an opt-in/configuration choice unless users actually control it.
   - Keep implementation-only rationale out of `README.md`, `RELEASE_NOTES.md`, and `facil_reference.yaml`.
   - Update `facil_reference.yaml` when public YAML configuration behavior, options, defaults, examples, or reference text change.
   - Before tagging, rename the release notes heading from `### Unreleased` to `### <Version> (<YYYY-MM-DD>)`, using the release date.
   - Inspect the versioned release notes before committing and tagging. The section should contain only intended public release notes, with no stale `Unreleased` heading and no private or implementation-only details.
   - Do not create or reuse the GitHub Release notes temp file yet; refresh it from the final tagged content after the successful NuGet push.

4. Verify locally. Mirror CI where practical, using the same release-critical environment variables and the Expecto console apps directly:

````powershell
$env:FACIL_FORCE_REGENERATE = "1"
$env:FACIL_FAIL_ON_CHANGED_OUTPUT = "1"
$env:connectionString = "<connectionString>"

dotnet tool restore
dotnet fantomas --check .
dotnet build src/TestDb/TestDb.sqlproj -c Release
dotnet sqlpackage /Action:Publish /SourceFile:"src/TestDb/bin/Release/TestDb.dacpac" /TargetConnectionString:"$env:connectionString"
dotnet build src/Facil.Package -c Release
dotnet pack src/Facil.Package -c Release
dotnet build src/PackageTests -c Release
dotnet src/PackageTests/bin/Release/net9.0/PackageTests.dll --fail-on-focused-tests
dotnet build src/DbTests -c Release
dotnet src/DbTests/bin/Release/net9.0/DbTests.dll --fail-on-focused-tests
````

   If a local SQL Server or connection string is unavailable, stop and report which CI-equivalent checks could not be run instead of replacing them with weaker `dotnet test` runs.

5. When packaging behavior or package metadata matters, inspect the produced `nupkg/Facil.<Version>.nupkg` metadata. Local pack can prove the package version and release-notes URL are embedded.

6. Commit and tag:
   - Load `git-commit-message` before authoring the commit.
   - Use `v/<Version>` for releases, for example `v/2.16.0`.

7. Push the commit and tag. Successful tag CI publishes the package to NuGet.

8. Watch the tag CI run and confirm NuGet publication before creating a GitHub Release:
   - Find the CI run for the pushed `v/<Version>` tag, not just the branch push run.
   - This repo's tag runs can be found with `gh run list --branch "v/<Version>"`:

````powershell
$tag = "v/<Version>"
$repo = "cmeeren/Facil"
$run = @(gh run list --repo $repo --workflow ci.yml --branch $tag --event push --limit 1 --json databaseId,headBranch,status,conclusion,url | ConvertFrom-Json)
if (-not $run) { throw "No CI run found for tag $tag" }
$runId = $run[0].databaseId
gh run watch $runId --repo $repo --exit-status
````

   - Confirm the tag run succeeded and that the `Push` step performed a real NuGet upload. The workflow uses `--skip-duplicate`, so a successful step can still mean the package already existed and was skipped. Inspect the fresh run logs; if the push was skipped as a duplicate, the push logs are unavailable, or NuGet publication is unclear, stop and inspect/report before creating a GitHub Release.
   - Verify the exact package version is visible on NuGet before creating a GitHub Release:

````powershell
$version = "<Version>".ToLowerInvariant()
$packageUrl = "https://api.nuget.org/v3-flatcontainer/facil/$version/facil.$version.nupkg"

for ($attempt = 1; $attempt -le 12; $attempt++) {
    try {
        Invoke-WebRequest -Method Head -Uri $packageUrl -UseBasicParsing | Out-Null
        break
    } catch {
        if ($attempt -eq 12) { throw "NuGet package is not available: $packageUrl" }
        Start-Sleep -Seconds 10
    }
}
````

9. Refresh and inspect the GitHub Release notes immediately before creating the release:
   - Copy the exact release section from the tagged `RELEASE_NOTES.md` to a temporary file under the user's temp directory, for example `$env:TEMP\facil-release-v<Version>.md` on PowerShell.
   - Prefer reading the tagged file with `git show "v/<Version>:RELEASE_NOTES.md"` so the GitHub Release notes match the commit that was packaged and published.
   - Manually inspect the temporary notes file. It should contain only the released version's notes, not the whole changelog and not a stale `Unreleased` heading.

10. Create the GitHub Release only after the successful NuGet push and notes inspection:

````powershell
$tag = "v/<Version>"
$repo = "cmeeren/Facil"
if (gh release view $tag --repo $repo 2>$null) { throw "GitHub Release already exists for $tag" }
gh release create $tag --repo $repo --verify-tag --title "Facil <Version>" --notes-file "$env:TEMP\facil-release-v<Version>.md"
````

   If the existing-release check finds a release, stop instead of creating a duplicate.

11. Verify the created release and remove the temporary notes file when practical:

````powershell
$tag = "v/<Version>"
$repo = "cmeeren/Facil"
gh release view $tag --repo $repo --json tagName,name,url
````

## Guardrails

- Do not start release steps from a dirty worktree.
- Do not create the GitHub Release before the tag CI has successfully pushed to NuGet.
- Use `gh release create --verify-tag` so GitHub does not auto-create or retarget the release tag.
- Use `--repo cmeeren/Facil` for `gh` release and run commands; do not rely on implicit `gh` repository context.
- Check for an existing GitHub Release for the tag before creating one; do not create duplicates.
- Refresh release notes from tagged content after NuGet publication; do not reuse an older temp file.
- Keep `README.md`, `RELEASE_NOTES.md`, and `facil_reference.yaml` public and user-facing.
- Update `facil_reference.yaml` whenever a release changes public YAML configuration behavior or reference material.
