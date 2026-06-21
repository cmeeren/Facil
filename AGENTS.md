# AGENTS.md

- For `src/DbTests/DbTests.fsproj`, prefer running the Expecto console app directly instead of `dotnet test`, especially for focused verification. The YoloDev.Expecto.TestSdk/VSTest adapter path can ignore or fail to apply filters and sit in the testhost for minutes.
  - Focused example: `dotnet src/DbTests/bin/Debug/net9.0/DbTests.dll --filter-test-case "Procedure/script parameter merge preserves buildValue fallback" --summary --no-spinner`
  - If the DLL is stale or missing, build first with `dotnet build src/DbTests/DbTests.fsproj`; note that building can refresh generated `src/DbTests.DbGen/DbGen.fs` metadata when the generator output changes.
  - Avoid using `dotnet test src/DbTests/DbTests.fsproj --filter ...` for local focused runs unless the task is specifically about the VSTest adapter.
