Facil
==============

<img src="https://raw.githubusercontent.com/cmeeren/Facil/master/logo/facil-logo-docs.png" width="300" align="right" />

**Facil generates F# data access source code from SQL queries and stored procedures. Optimized for developer happiness.**

Pro-tip: Facil works great with [Fling](https://github.com/cmeeren/Fling)!

Quick start
-----------

### 0. Requirements

* SQL Server 2012 or later at build-time (may work with older versions at runtime; untested)
* .NET 6.0 or later at build-time (for running the generator)

### 1. Install

Install Facil from [NuGet](https://www.nuget.org/packages/Facil).

### 2. Edit the new config file

You now have [a simple facil.yaml](https://github.com/cmeeren/Facil/blob/master/src/Facil.Generator/facil_minimal.yaml) file in your project directory (if not, build your project or just add it manually). Edit it – you should at least set a connection string.

As an example, the following is the minimum config for generating code for all stored procedures in the database and all scripts in your project directory:

```yaml
rulesets:
  - connectionString: YOUR CONNECTION STRING HERE
    procedures:
      - include: .*
    scripts:
      - include: "**/*.sql"
```

You might consider adding the config file to your project for easy access in Visual Studio.

For more details about configuration, you can find [the full config reference here](https://github.com/cmeeren/Facil/blob/master/facil_reference.yaml).

### 3. Build your project and add the generated file

Build your project again. Facil will now generate the code. Add the generated file to your project.

### 4. Use the generated code

For example:

```f#
open DbGen.Procedures.dbo

let getUser (connStr: string) (UserId userId) : Async<User option> =
  async {
    return!
      GetUserById
        .WithConnection(connStr)
        .WithParameters(userId)
        .AsyncExecuteSingle()
  }
    

let searchProducts (connStr: string) (args: ProductSearchArgs) : Async<ResizeArray<Product>> =
  async {
    let dtoWithPrimitiveParams = ProductSearchArgs.toDto args
    return!
      GetUserById
        .WithConnection(connStr)
        // You can load parameters from any object with the right members
        // instead of passing each parameter manually
        .WithParameters(dtoWithPrimitiveParams)
        .AsyncExecute()
  }
```

### 5. Profit!

That’s it! Whenever you want to regenerate, simply delete the first line of the generated file and build your project.

Facil will also regenerate on next build if you change the configuration or any scripts. For details, see the FAQ entry [When does Facil regenerate files?](#when-does-facil-regenerate-files).

## Elevator pitch

Facil is a friendly, flexible, and full-featured fabricator of files for fluent and frictionless facades facilitating the facile and functional fetching of facts.

(“Facts” referring to data stored in SQL server. It would be better if SQL started with an F. Oh well.)

Okay, elevator pitch without the alliteration: Facil works similarly to type providers like [FSharp.Data.SqlClient](https://github.com/fsprojects/FSharp.Data.SqlClient/) by letting you call SQL scripts and stored procedures in a strongly typed manner, but it avoids a range of type provider issues and hiccups by not being a type provider and actually generating F# code that you check in. (Why not a type provider, you ask? [See the FAQ.](#why-not-a-type-provider))

#### Core features

* Primary goal: Provide the simplest way (yet highly configurable) to call SQL scripts and stored procedures as if they were F# functions, and otherwise get out of your way and let you get on with providing actual business value
* Can also [generate simple per-table CRUD scripts](#can-facil-generate-sql-scripts)
* Good API ergonomics – succinct and discoverable fluent-style syntax, no boilerplate
* Supports SQL Server 2012 and up
* Built for the future, compatible with .NET 6.0 and later
* Thoroughly tested
* Built for speed – inner async read loops written in C# using its native `async`/`await`; allows you to read directly to your chosen DTO records to minimize allocations for heavy queries
* Highly configurable with simple, yet flexible [YAML configuration](https://github.com/cmeeren/Facil/blob/master/facil_reference.yaml)
* Helpful build-time error messages and warnings
* At runtime, supply a connection string for simplicity or use your own managed connections
* Can be configured to use `ValueOption` instead of `Option` (separately for inputs and outputs, for some or all procedures/scripts)
* Can create DTOs for tables and automatically (or manually) use those DTOs as return values for matching result sets, simplifying your mapping to domain entities
* Can also map directly to any record type you specify (as mentioned previously)
* Can accept suitable DTOs instead of a list of parameters, e.g. you can just pass your `UserDto` to your `SaveUser` procedure instead of explicitly supplying all parameters from the DTO – less noise, and one less thing to update each time you add parameters
* Supports table-valued parameters in both procedures and scripts
* Supports stored procedure output parameters and return values
* Supports lazy execution, both sync (returns `seq`) and async (the latter returns `IAsyncEnumerable`, use with e.g. [FSharp.Control.AsyncSeq](https://github.com/fsprojects/FSharp.Control.AsyncSeq))
* Supports inferring dynamic SQL result sets *without* `WITH RESULT SETS`
* To some extent [checks your dynamic SQL at build time](#can-facil-check-my-dynamic-sql)
* Supports [temp tables](#can-i-use-temp-tables)

### Production readiness

Facil is production ready.

Facil contains over 2000 tests verifying most functionality in many different combinations, and is used in several mission-critical production services at our company. I’m not claiming it’s perfect, or even bug-free, but it’s well tested, and I have a vested interest in keeping it working properly.


FAQ
---

### Can Facil generate SQL scripts?

Yes, Facil can automatically generate the following simple per-table CRUD scripts, saving you from both writing and configuring them:

* Insert a row (supports output columns)
* Update a row by its primary key (supports output columns)
* “Upsert” - use MERGE to insert or update a row by its primary key (supports output columns)
* Delete a row by its primary key (supports output columns)
* Get a row by its primary key
* Get rows by a batch of primary keys (using a TVP)
* Get a row by a set of arbitrary columns
* Get rows by a batch of arbitrary columns

For details, see [the full config reference](https://github.com/cmeeren/Facil/blob/master/facil_reference.yaml) and search for `tableScripts`.

### Ooh, neat! Can you auto-generate this and that script too?

Probably not.

Facil’s primary focus is allowing you to call your existing TSQL in the simplest fashion possible. There are no plans on adding more queries or options than the ones currently implemented. Adding ever new script types or making the existing ones more flexible is a bottomless rabbit hole of scope creep, so I have to draw the line somewhere.

I welcome suggestions, but due to my limited capacity for open-source maintenance, any additions or improvements will likely need a very high utility-to-maintenance ratio, or scratch a personal itch of mine.

If the above queries with the available configuration options don’t satisfy your needs, you can, after all, just write the queries manually and consume them using Facil.

### Why not a type provider?

Type providers are great in theory, and to a large extent also in practice, but have some notable drawbacks:

* Potentially horrible IDE performance if you use it a lot, killing your productivity
* No standardized schema caching or “offline” mode where it will use the already generated types without hitting your DB; any offline implementation is entirely up to the type provider implementer, meaning support may be hit-and-miss or completely missing 
* Not necessarily CI/CD friendly; you may be required to ensure that the build server has access to a suitable database, either destroying repeatable/parallel builds (if using an external database) or placing cumbersome constraints on the build agents (if using a DB on the build agent), and in all cases lengthening the build time
* Limitations for what kind of types can be provided (no union types being a well-known example, though not itself relevant for Facil)
* May need to reload projects to force the TP to update after changes to external schema

Facil, by generating plain old F# code that you check in, sidesteps all of these issues (but still optionally supports forced generation on CI and also optionally failing the build if the generated code has changed).

### When does Facil regenerate files?

Facil will regenerate (hitting your DB) before the compilation step of your build if any of the following are true:

* There are changes to the two first lines of the generated file(s) (this is the simplest way to manually force a rebuild)
* There are changes in included SQL scripts
* There are changes in the config file
* There are changes to Facil itself (i.e., when updating Facil)
* The environment variable `FACIL_FORCE_REGENERATE` exists

If none of the above are true, Facil will skip its build step and thus not hit your DB.

Notably, this means that Facil **will not pick up changes to your database**. This is by design. If you update the DB that Facil connects to during build, just open the generated file and delete the first line and Facil will re-generate it on the next build.

### Can I force Facil to run during CI build and fail if the generated file is not up to date?

Yes. There are two environment variables you can set. You can use either of them on their own, or together:

* `FACIL_FORCE_REGENERATE`: Set this to force Facil to always regenerate during build. Alone, this variable will effectively make Facil mimick a type provider without caching/offline capabilities.
* `FACIL_FAIL_ON_CHANGED_OUTPUT`: Set this to make Facil fail the build if the output has changed. You can use this to reject commits that does not include up-to-date generated code.

### What can I configure?

See [the full YAML config reference for details](https://github.com/cmeeren/Facil/blob/master/facil_reference.yaml). Note that the top-level `rulesets` property is an array, meaning you can generate multiple source files with separate configs (e.g. to generate from multiple DBs) simply by adding another array item with the desired configuration (see the bottom of the reference YAML for details). Here are some highlights of what you can configure.

For each file, you can configure:

* The generated filename
* The generated namespace/module
* Arbitrary code to put at the top of the generated file
* Which stored procedures (regex matching) or scripts (glob matching) to generate code for
* Which tables to generate DTO records for (which can be used, automatically or manually, for matching procedure/script result sets)
* The base path for all scripts glob patterns (can be outside your project directory)

For each procedure/script (or any set of these that matches a specified regex/glob pattern), you can configure:

* The result type: Anonymous record, auto-pick best table DTO (with fallback to anonymous records), or any record type you specify (a table DTO or your own custom type) that is constructible by the generated code
* Whether to use `ValueOption` instead of `Option` for inputs and/or outputs
* For single-column results, whether to return a record (as with multiple columns) or just return the scalar value
* Whether to skip the `inline` DTO parameter overloads (for faster compilation if you don’t use them)
* Whether to use return values (stored procedures only)
* For each parameter: Its nullability and the name to use in parameter DTO objects
* For each script parameter: Its type (to work around type inference limitations for scripts, [see below](#type-inference-limitations-in-scripts))
* For scripts: temp tables ([see below](#can-i-use-temp-tables))

For each table DTO, you can configure:

* Whether to use `ValueOption` instead of `Option`

For each table type (automatically included by Facil if used in included procedures/scripts), you can configure:

* Whether to use `ValueOption` instead of `Option`
* Whether to skip the `inline` DTO parameter overloads (for faster compilation if you don’t use them)

### Type inference limitations in scripts

Type inference in scripts is limited due to limitations in SQL Server's [sp_describe_undeclared_parameters](https://docs.microsoft.com/en-us/sql/relational-databases/system-stored-procedures/sp-describe-undeclared-parameters-transact-sql), which Facil uses to get parameter information for scripts. Notably, the following does not work out of the box:

* Parameters used multiple times will give errors
* Table-valued parameters will give errors
* Nullability is not inferred (by default, Facil assumes all script parameters are non-nullable)

To work around this, for each problematic parameter (you don't have to specify the ones that work), you can specify in the config which SQL type the parameter is and whether it is nullable. You can also set this for all parameters at once (and override specific parameters).

### How are default and nullable parameter values handled?

All stored procedure parameters that have `null` as the default value are treated as nullable and wrapped in `option` (or `voption`). All other default values for stored procedure parameters are ignored; the parameters will be required and non-nullable.

While parameters with default values could conceivably be generated as optional method parameters, this runs the risk of forgetting to use them when executing the procedure (I’ve been burned by this a lot), and it also won’t mesh that well with the parameter DTO overloads.

If Facil’s current approach does not work for you, please open an issue and describe your use-case.

### Can Facil support user-defined functions?

If you need this, I’m willing to hear you out, but I have limited OSS maintenance resources and this isn’t high on my priority list right now. A simple workaround is to simply call the function from a script or stored procedure, and then use Facil with that script/procedure instead.

### Does Facil use column names or ordinals?

Facil uses column names at runtime. This means that you are free to reorder the columns returned by stored procedures and scripts (either explicitly, or by reordering table columns returned in a `SELECT *` query). This does not require re-compilation and will not break existing running apps.

### Can I use temp tables?

Yes, Facil supports temp tables in scripts. In short, configure your script like this:

```yaml
scripts:
  - include: "MyScriptUsingTempTables.sql"
    tempTables:
      # You can supply the definition directly as a string.
      - definition: |
          CREATE TABLE #myTempTable1(
            Col1 INT NOT NULL PRIMARY KEY,
            Col2 NVARCHAR(100) NULL
          )

      # If using a single line, remember to enclose in quotes since '#' starts a YAML comment
      - definition: "CREATE TABLE #myTempTable2(Col1 INT NOT NULL)"

      # You can also specify a path to a SQL file containing the definition
      - definition: path/from/project/to/myTempTable3.sql
```

Then temp tables will work similarly to TVPs:

```f#
MyScriptUsingTempTables
  .WithConnection(connStr)
  .WithParameters(
    tempTable1 = [
      MyScriptUsingTempTables.tempTable1.create(
        Col1 = 1,
        Col2 = Some "test"
      )
    ],
    tempTable2 = [
      MyScriptUsingTempTables.tempTable2.create(
        Col1 = 1
      )
    ],
    ..
  )
```

Just like with TVPs, you can use matching DTOs in the calls to `create` (instead of explicitly passing column parameters as shown above).

Facil uses [`SqlBulkCopy`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlbulkcopy) to load temp tables. You can configure the created `SqlBulkCopy` using `ConfigureBulkCopy`.

```f#
MyScriptUsingTempTables
  .WithConnection(connStr)
  .ConfigureBulkCopy(fun bc ->
    bc.BatchSize <- 1000
    bc.BulkCopyTimeout <- 0
    bc.NotifyAfter <- 2000
    bc.SqlRowsCopied.Add (fun e -> printfn "%i rows copied so far" e.RowsCopied)
  )
  .WithParameters(..)
```

The configuration will apply to the loading of all temp tables for the script; please open an issue if you need separate configuration per temp table.

### Why do the `Execute` methods return `ResizeArray` and not an F# `list`?

The rows have to be read from the DB one at a time without knowing how many rows there are. As far as I know, a `ResizeArray<_>` (an alias for `System.Collections.Generic.List<_>`) generally provides the most efficient way (at least in terms of allocations) to build up a collection with an unknown number of items, requiring one allocation and array copy each time the internal array is resized. An F# `list` would cause one allocation per cell (item), though as far as I know, it would not require any copies (though it would be built up in reverse and therefore require a full traversal when reversing the list at the end).

F# users would normally want a `list` instead of a `ResizeArray`, but you can get that trivially by just calling `Seq.toList` on the result. This is similar to e.g. FSharp.Data.SqlClient. Facil could provide `Execute` variants that do this for you, but then you’d have twice as many `Execute` methods to choose from, which would add confusion, and the name prefix/suffix would almost be as verbose as just calling `Seq.toList` yourself.

If you think that building up a `list` directly in the read loop would be more efficient as it would avoid the “copy” cost of `Seq.toList`, then 1) I don’t think that’s correct, because (as mentioned above) an F# `list` would have to be built up in reverse by prepending each item, and the `List.rev` at the end would cause at least one “copy” anyway, and 2) in the rare case that your use-case is so sensitive to performance that you are concerned about the performance impact of `Seq.toList`, then you should probably just just use the returned `ResizeArray` directly.

Note that since Facil is a general-purpose data access library, I do not know anything about user workloads, databases or connections, and I have not benchmarked anything. All of the above is going by intuition (admittedly a dangerous thing in the performance world) as well as a desire to keep the internals fairly simple.

If you believe any of the above is incorrect and have either sound arguments or proper benchmark data (e.g. using [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)) to back it up, please open an issue and we can discuss both the public API and the implementation details.

### Can Facil support SSDT (`.sqlproj`) projects, like [SQLProvider](http://fsprojects.github.io/SQLProvider/)?

No, unfortunately that’s not feasible.

SQLProvider uses information about tables and views and lets you write F# queries against them; it doesn’t provide strongly typed access to stored procedures or scripts. This means that SQLProvider “only” has to know the table/view schema, which (at least for tables) can be easily parsed directly from the definition.

Facil, on the other hand, must know the input parameters and output columns of scripts and stored procedures. While it would be fairly easy to get the input parameters of stored procedures by just parsing the procedure definition, Facil has to use SQL Server functionality on the deployed schema to get script input parameters (`sp_describe_undeclared_parameters`) and script/procedure output columns (`sp_describe_first_result_set` and `SET FMTONLY ON`).

The only way for Facil to be able to use SSDT projects directly would be if this SQL type inference functionality was implemented directly in the Facil generator. For all but the most trivial schema, this seems too complicated to be worth the implementation and maintenance effort. Therefore, Facil only works with deployed schema.

The most significant drawback of this limitation is that it is possible to update the SSDT schema but forget to deploy and re-generate. However, that is no different than if you were using [FSharp.Data.SqlClient](http://fsprojects.github.io/FSharp.Data.SqlClient/), and can be alleviated by using `FACIL_FORCE_REGENERATE` and `FACIL_FAIL_ON_CHANGED_OUTPUT` on your build server.

A bonus, however, is that having to deploy the updated schema to a database during development ensures that the schema actually “compiles”.

### Why does Facil not generate result sets for my dynamic SQL query?

TL;DR: Use `buildValue` in the parameter config for parameters with sort column names etc.

Long version: This can happen when all of the following are true:

* You use dynamic SQL
* The syntax of the dynamic SQL statement is sensitive to the value of a parameter (e.g. a parameter contains the column name to sort by)
* You are not using `WITH RESULT SETS`
* You are not using `buildValue` for the relevant parameters in `facil.yaml`

In this scenario, the output column parser returns no columns. (Unfortunately Facil can’t easily find out whether this is an error or if the script actually returns no results, and thus can’t display a warning/error instead of silently producing non-query code.)

The fix is simple: In the procedure/script config, add a parameter with `buildValue` set to a value you know will work:

```yaml
- for: MyProcedure
  params:
    mySortColParam:
      buildValue: MySortColumn
```

Another workaround is to use `WITH RESULT SETS`. This will in many cases enable Facil to use another method of determining the output columns that does not depend on parameter values. However, compared to using `buildValue`, it is more likely to break (since you must keep it in sync with the `SELECT` list) and likely more verbose.

### Can Facil check my dynamic SQL?

Facil provides some checking of dynamic SQL as long as you don’t use `WITH RESULT SETS`.

As an example, take the following script:

```sql
DECLARE @sql NVARCHAR(MAX) = '
SELECT * FROM dbo.MyTable
WHERE
  1 = 1
'

IF @col1Filter IS NOT NULL
  SET @sql += '
  AND Col1 = @col1Filter
'

IF @requireCol2Zero = 1
  SET @sql += '
  AND Col2 = 0
'

DECLARE @paramList NVARCHAR(MAX) = '
  @col1Filter NVARCHAR(100)
'

EXEC sp_executesql @sql, @paramList, @col1Filter
```

In order to parse the output columns of dynamic SQL queries, Facil must execute your query and see which columns come back. At build time, Facil generally passes `1` (or `"1"` etc.) for all parameters when executing the query. In the common case of dynamic filters as shown above, where you use `IS NOT NULL` or `@param = 1` to add filters to the executed SQL, this means that your dynamic SQL will be executed with all the optional filters.

Facil may not completely check your dynamic SQL. For example, you may have a parameter that is used to choose one of several different `ORDER BY` clauses. In this case, only one of them will be used at build time (and you may be able to specify the parameter value by using `buildValue` as described previously).

### Can Facil make it easy to save/load domain entities to/from multiple tables?

This is exactly what [Fling](https://github.com/cmeeren/Fling) does. Fling works great with Facil!

Release notes
-------------

[RELEASE_NOTES.md](https://github.com/cmeeren/Facil/blob/master/RELEASE_NOTES.md)

### A note on versioning

Facil follows [SemVer](https://semver.org/).

Note on what a “breaking change” is: A lot of the generated code needs to be public to support inlining, but is still considered implementation details. These parts of the API are hidden from the IDE using the `[<EditorBrowsable>]` attribute to ensure you won’t use them by accident, but there’s nothing stopping you from looking at the generated code and referencing these parts of the API in your own code. Don’t do that. These are implementation details and may change at any time.

Contributing
------------

Contributions and ideas are welcome! Please see [Contributing.md](https://github.com/cmeeren/Facil/blob/master/.github/CONTRIBUTING.md) for details.
