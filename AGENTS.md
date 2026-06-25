# AGENTS.md

- For `src/DbTests/DbTests.fsproj`, prefer running the Expecto console app directly instead of `dotnet test`, especially for focused verification. The YoloDev.Expecto.TestSdk/VSTest adapter path can ignore or fail to apply filters and sit in the testhost for minutes.
  - Focused example: `dotnet src/DbTests/bin/Debug/net10.0/DbTests.dll --filter-test-case "Procedure/script parameter merge preserves buildValue fallback" --summary --no-spinner`
  - If the DLL is stale or missing, build first with `dotnet build src/DbTests/DbTests.fsproj`; note that building can refresh generated `src/DbTests.DbGen/DbGen.fs` metadata when the generator output changes.
  - Avoid using `dotnet test src/DbTests/DbTests.fsproj --filter ...` for local focused runs unless the task is specifically about the VSTest adapter.
- Release procedure: use the repo-local `release-facil` skill.
- For user-relevant changes, add a concise entry in the `### Unreleased` section in `RELEASE_NOTES.md`, creating that section if it does not exist. Use categorizing subsections such as `#### Added` or `#### Fixed` only when the unreleased notes are large enough that grouping materially improves readability.
- In release notes, describe the observable effect from the user's perspective. Avoid naming internal mechanisms or implying an opt-in/configuration choice unless users actually control it.
- Keep `README.md`, `RELEASE_NOTES.md`, and `facil_reference.yaml` user-facing. Describe behavior, migration impact, usage guidance, and YAML configuration reference details; avoid implementation details, internal-only rationale, and irrelevant churn.
- Update `facil_reference.yaml` when public YAML configuration behavior, options, defaults, examples, or reference text change.
