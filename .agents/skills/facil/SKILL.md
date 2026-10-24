---
name: facil
description: Configure Facil and call the F# data access code it generates from SQL scripts and stored procedures. Use when writing or changing facil.yaml, SQL scripts or procedures that Facil generates code for, table DTOs or table scripts, when regenerating Facil output, or when writing code that calls Facil-generated APIs.
---

# Facil

This skill describes Facil 3.x. Use guidance matching the project's package version and follow the project's existing Facil configuration and conventions. The links below track development; for released versions, read the same files at Git tag `v/<version>` instead of `main`.

Facil generates F# code for SQL scripts and stored procedures by asking SQL Server at build time for their parameters and result columns. The [config reference](https://github.com/cmeeren/Facil/blob/main/facil_reference.yaml) documents every `facil.yaml` setting and its default; read the relevant part before adding or changing settings. The [README FAQ](https://github.com/cmeeren/Facil/blob/main/README.md#faq) covers usage details and limitations.

## Regeneration

Facil regenerates its output file, such as `DbGen.fs`, before compilation when `facil.yaml`, an included script or SQL fragment, the generated file's header, or Facil itself changes, or when the `FACIL_FORCE_REGENERATE` environment variable is set. It does not notice database schema changes: after changing the schema, set `FACIL_FORCE_REGENERATE` for that build or delete the generated file's first line. If the project doesn't rebuild after changing only `facil.yaml` or SQL files, add them as `UpToDateCheckInput` items (see the README).

Regeneration reads the schema of the database in the ruleset's `connectionString`, so that database must have the schema the code targets. Review the regenerated diff: changes you didn't intend usually mean that database differs from the intended schema. Keep the generated file in source control.

Never edit generated code; change the SQL or `facil.yaml` instead. Don't reference code that Facil itself marks with `[<EditorBrowsable(EditorBrowsableState.Never)>]`; it is an implementation detail that can change in any release. The same attribute on a module that the project's own `namespaceOrModuleDeclaration` declares doesn't make that module off-limits.

## Configuration

A ruleset generates one file from one database. Its `procedures`, `scripts`, `tableDtos` and `tableScripts` rules use `include` to add items and `for` to configure items without adding them. A rule's `except` narrows only that rule, so an item that another `include` rule matches is still included. All matching rules apply in order, later values overriding earlier ones; `tempTables` lists replace earlier ones rather than merging. Procedure, table and table type patterns are case-sensitive .NET regexes matched against schema-qualified names, so anchor them: `dbo\.User` also matches `dbo.UserRole`, while `^dbo\.User$` does not. Script patterns are globs relative to `scriptBasePath`. Script rules match table scripts by their `name`, such as `User_ById` without `.sql`, so a rule like `for: '**/*.sql'` doesn't configure them.

Keep secrets out of `facil.yaml` by using `$(Name)` variables resolved from the `configs` sources (appsettings, user secrets, environment variables).

Prefer configuration that keeps Facil inferring what it can:

- **Script parameters:** SQL Server can't infer the type of a parameter used more than once, of one only passed to dynamic SQL, or of a table-valued parameter, and Facil assumes every script parameter is non-nullable. Set `type` only for those parameters (a schema-qualified table type name for TVPs), and `nullable: true` for parameters that accept `NULL`. Procedure parameters are nullable when their default is `NULL`; other defaults are ignored, so callers pass those parameters explicitly. A parameter's `nullable` setting overrides either default.
- **Result types:** With the default `result: auto`, a result set uses the table DTO whose columns it matches exactly, ignoring order: same names, types and nullability. Otherwise it silently gets an anonymous record, and if several DTOs match, Facil warns and also uses an anonymous record. Generate table DTOs with `tableDtos` (narrowed with `includeColumns` where needed) for rows you map to domain types. When code depends on a DTO, name it with `result: <schema>.<table>`, so the build fails if the result set stops matching instead of silently switching type. Joins and expressions often change a column's nullability or type; prefer fixing the SQL, such as with `ISNULL` or an inner join, since a column's `nullable: false` override isn't checked and fails at runtime when the value is `NULL`. Use `mappingCtor` for results with columns beyond a DTO's, `result: nominal` for a named record per script, or a qualified record type of your own. Facil only uses the first result set, and a single-column result returns bare values unless `recordIfSingleCol: true`.
- **Table scripts:** Use `tableScripts` for single-table CRUD and lookups by key or columns, including batch variants, instead of handwritten SQL. Script rules can configure them, except their parameters, which need no configuration. Batch lookups need a table type matching the key or filter columns; set `tableType` when several match.
- **Shared SQL:** Use `expandIncludes` with `{{include "relative/path.sql"}}` and exclude the fragments from the script globs.
- **Dynamic SQL:** At build time Facil executes dynamic SQL in a rolled-back transaction to find its result set, passing placeholder values that are non-null except for `geometry` and `geography` parameters. For a parameter that changes the SQL's shape, such as a sort column name, set a valid `buildValue`, or Facil may generate code without a result set.
- **Temp tables:** Declare the definitions the script expects under `tempTables`; Facil loads them with `SqlBulkCopy`, which `ConfigureBulkCopy` configures.
- **Options:** `voptionIn` and `voptionOut` switch nullable values to `ValueOption`; a result with nullable columns only auto-matches table DTOs whose `voption` equals the script's `voptionOut`. `paramDto` controls the `WithParameters` overload that takes an object: `inline` (default) accepts any object with matching members, `nominal` only the generated `<Name>_Params` record, and `skip` removes the overload, which can speed up compilation of large generated files. `dateType: dateTime` keeps legacy `DateTime` for SQL `date`.

## Calling generated code

Generated types live under the ruleset's module or namespace, by default `DbGen`: `DbGen.Scripts`, in modules nested by the scripts' folders under `scriptBasePath`, and `DbGen.Procedures.<schema>`, `DbGen.TableDtos.<schema>` and `DbGen.TableTypes.<schema>`. A call chains `WithConnection`, optionally `ConfigureCommand`, `WithParameters` and an execution method:

```fsharp
open DbGen.Scripts

let getUser (connStr: string) (userId: int) =
    User_ById.WithConnection(connStr).WithParameters(userId).AsyncExecuteSingle()
```

- `WithConnection` takes a connection string, which Facil opens and disposes, or an open `SqlConnection` and optional `SqlTransaction`, which stay yours to dispose. Pass the transaction only there, not through `ConfigureCommand`.
- `WithParameters` takes the parameters as method arguments. Name them when there are several: script parameters follow their first use in the SQL, so editing the SQL can otherwise silently swap arguments of the same type. Depending on `paramDto`, it also takes an object, such as a table DTO. TVP and temp-table rows are created with the generated `create` functions.
- For queries, `Execute`, `ExecuteAsync` and `AsyncExecute` return all rows as a `ResizeArray`, and the `ExecuteSingle` variants return the first row as an option (`voption` with `voptionOut`). Commands without a result set return the affected row count. With output parameters or `useReturnValue`, the result is wrapped in a record with `Result` and `Out` and/or `ReturnValue`. `LazyExecute` and `LazyExecuteAsync` stream rows, and the `ExecuteReader` variants return a `FacilReaderDisposer` to bind with `use`. Output parameters and procedure return values are unavailable from lazy and reader execution.
- Facil reads result columns by name, so reordering a query's columns doesn't break deployed code.
