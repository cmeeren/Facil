Release notes
==============

### Unreleased

* Fixed missing primary key in temp table for `mergeBatch` table scripts

### 2.6.0 (2022-12-10)

* Added batch insert/update/merge table scripts (`insertBatch`, `updateBatch`, and `mergeBatch`)

### 2.5.8 (2022-11-02)

* Fixed a build-time error for certain queries when using connection strings with credentials but without `Persist Security Info=true`
* Updated Microsoft.Data.SqlClient from 5.0.0 to 5.0.1

### 2.5.7 (2022-08-08)

* Reverted some package changes in the generator to avoid an [upstream bug](https://github.com/dotnet/runtime/issues/65756)

### 2.5.6 (2022-08-08)

* Updated Microsoft.Data.SqlClient from 4.1.0 to 5.0.0

### 2.5.5 (2022-07-12)

* Fixed rare bug where generation failed for stored procedures that used temp tables and required execution to infer
  the result set (i.e., where neither `sp_describe_first_result_set` nor `SET FMTONLY ON` could be used)
  ([#39](https://github.com/cmeeren/Facil/issues/39))

### 2.5.4 (2022-07-01)

* Now warns and ignores scripts with multiple columns where at least one is unnamed, instead of throwing NullReferenceException 

### 2.5.3 (2022-06-21)

* Fixed false negative config change detections (where regeneration was skipped even if the config was changed)
* Now warns when encountering non-existent columns in table scripts

### 2.5.2 (2022-05-24)

* Fixed false negative table type compatibility result

### 2.5.1 (2022-05-24)

* Fixed regression introduced in 2.4.0 where Facil would trigger regeneration in different environments even if there were no changes in the config  

### 2.5.0 (2022-05-24)

* Column order is now ignored when finding matching table types for table scripts
* Column order is now ignored when finding matching table DTOs for script output
* Fix compilation error when a script or procedure returns a table DTO whose name or schema is not a valid F# identifier

### 2.4.0 (2022-05-24)

* If you generate multiple files (i.e., have multiple rulesets), when you modify the YAML file, Facil will now only regenerate the files corresponding to the changed rulesets 
* You can now use `includeColumns` as syntactic sugar in `tableDto` configurations

### 2.3.2 (2022-03-03)

* Reverted some package changes in the generator to avoid an [upstream bug](https://github.com/dotnet/runtime/issues/65756)

### 2.3.1 (2022-03-01)

* Fixed erroneous build-time warning 
* Updated Microsoft.Data.SqlClient from 4.0.0 to 4.1.0

### 2.3.0 (2022-01-12)

* Now supports temp tables for procedures (in addition to scripts)

### 2.2.1 (2022-01-12)

* Table scripts may now use columns that are explicitly skipped in a table DTO for the same table

### 2.2.0 (2022-01-05)

* Added an optional `SqlTransaction` parameter to the `WithConnection` overload accepting a `SqlConnection`. This is now the preferred method of setting a transaction, and may be required for future functionality (see e.g. below). The old one (using `ConfigureCommand`) will continue to work, but an exception will be thrown if both methods are used. 
* Scripts using temp tables now work with transactions (requires using the new `WithConnection` overload to set the transaction)

### 2.1.2 (2021-12-16)

* Fixed a regression in 2.1.1 where table scripts were missing if the table was not included in a table DTO rule 

### 2.1.1 (2021-12-15)

* Fixed a bug where generation would fail if the database contains a table that is not actually included, and the table has unsupported columns
* Generation now uses a rolled-back transaction when it needs to execute procedures and scripts, so that the database is not changed during build-time even if the script makes changes

### 2.1.0 (2021-12-14)

* Added a `getAll` table script type that simply selects all rows from a table (`SELECT Col1, Col2 FROM TableName`)

### 2.0.0 (2021-11-29)

* Now targets only .NET 6 (both for build-time and run-time)
* Updated Microsoft.Data.SqlClient from 2.1.0 to 4.0.0. For details on which breaking changes this entails, see the package's [3.0.0 release notes](https://github.com/dotnet/SqlClient/blob/main/release-notes/3.0/3.0.0.md) and [4.0.0 release notes](https://github.com/dotnet/SqlClient/blob/main/release-notes/4.0/4.0.0.md). Notably, connections are now encrypted by default.

### 1.4.3 (2021-09-02)

* Fix build-time bug where scripts starting with a CTE (`WITH`) and having parameter types configured in `facil.yaml` required a semicolon before `WITH` to build properly. (The semicolon was required because configured parameter types causes Facil to insert parameter definitions at the start of the script during build. Facil now inserts the required semicolon automatically during this process.)

### 1.4.2 (2021-06-18)

* Added `paramDto` config key to procedures and scripts, with values `inline` (default), `nominal` (generate a nominal input DTO type, similar to what `result: nominal` does for output), and `skip` (replacing the old `skipParamDto`).
* Deprecated `skipParamDto`; use `paramDto: skip` instead.

### 1.4.1 (2021-04-30)

* Will now always skip computed columns in insert/update/merge table scripts

### 1.4.0 (2021-04-16)

* Will now silently ignore insert/update/merge/delete table scripts for matched views
* Procedure parameters now support `nullable` overrides, just like script parameters

### 1.3.3 (2021-04-07)

* Changed the output format of script source code to clean up and improve telemetry that picks up the script source (specifically, modified the script source indentation and inserted the script name in a comment at the top of the script)

### 1.3.2 (2021-03-19)

* Fixed table script merge (now merges based on the final name, not the template name with `{TableName}` and other tokens)

### 1.3.1 (2021-03-19)

* Fixed build targets on Linux

### 1.3.0 (2021-03-19)

* Added ability to generate `MERGE` table script (“upsert”)
* Added `selectColumns` as syntactic sugar for output columns in `Get*` table scripts

### 1.2.2 (2021-03-18)

* `getByColumns` and `getByColumnsBatch` table scripts now require non-null parameters even if filtering on nullable columns

### 1.2.1 (2021-03-18)

* Changed the new `getPrimaryKey` function from a module function to a static member on the table DTO type to avoid conflicts when there are table DTOs that only differ by one of them ending with `Module`

### 1.2.0 (2021-03-18)

* Add a table DTO module alongside each table DTO containing a function to get the DTO’s primary key (if none of the PK columns are skipped)

### 1.1.0 (2021-03-17)

* Added a public constructor to all script/procedure entry types to help reflection/SRTP (the constructor throws if called)

### 1.0.1 (2021-03-16)

* Fix regression causing false negatives in check for whether a table DTO can be returned by a script/procedure
* Fix bug where table DTO could not be used as a result set if it was explicitly specified using  `result`

### 1.0.0 (2021-03-16)

* Facil can now automatically generate the following simple per-table CRUD scripts, saving you from both writing and configuring them:
  * insert
  * update
  * delete
  * get by primary key
  * get batch by primary key
  * get by a set of arbitrary columns
  * get batch by a set of arbitrary columns

### 0.2.9 (2021-03-02)

* `tableDtos` now includes views in addition to tables

### 0.2.8 (2021-02-23)

* Improved error message when lacking permission to view the definition of a stored procedure.

### 0.2.7 (2021-02-15)

* Adjust generated code to avoid stack overflow when there are very many (several hundreds) nullable output columns, due to particularities of the F# compiler ([#9](https://github.com/cmeeren/Facil/issues/9)/[#10](https://github.com/cmeeren/Facil/issues/10), thanks [@davidtme](https://github.com/davidtme))

### 0.2.6 (2021-02-11)

* Make the generated code more compatible with older F# versions

### 0.2.5 (2021-02-10)

* Allow setting `result` to `nominal` for procedures and scripts, which will generate and use a normal F# record for the result set.

### 0.2.4 (2021-02-04)

* Made async computations for parametrized operations directly retryable (see [#6](https://github.com/cmeeren/Facil/issues/6) for details)

### 0.2.3 (2021-01-15)

* Fixed parameter and column inheritance

### 0.2.1 (2021-01-15)

* Fixed bug where case-transformed table DTOs used as outputs used incorrect casing

### 0.2.0 (2021-01-15)

* **Breaking:** Table DTO fields are now PascalCase so they can be easily used in DTO `WithParameter` overloads even when table columns are camelCase.

### 0.1.16 (2021-01-13)

* Fixed exception when passing empty TVPs

### 0.1.15 (2021-01-12)

* Automatically support dynamic SQL queries with full-text predicates (previously required `buildValue`)
* Generally check more dynamic SQL by executing script with non-zero/non-empty parameter values

### 0.1.14 (2021-01-12)

* Fixed bug where MAX length parameters in TVP would throw exception

### 0.1.13 (2021-01-08)

* The default generated file module is now public instead of internal (since internal table DTOs can’t be used in the DTO `WithParameter` overloads).
* Fixed bug where table DTOs with non-F#-friendly column names would fail

### 0.1.12 (2021-01-06)

* Make change detection more resilient against environment differences

### 0.1.11 (2021-01-06)

* Fix false positive change detection from v0.1.9

### 0.1.10 (2021-01-06)

* Ignore line endings when comparing script sources for determining whether to regenerate

### 0.1.9 (2021-01-06)

* Facil will no longer regenerate if connection string variable contents change. This was fundamentally flawed, since it meant that if you don’t want to regenerate on CI, you’d have to set up a variable with the exact same connection string you use locally. Now you don’t have to set up any variables on CI if you don’t intend to regenerate.

### 0.1.8 (2021-01-06)

* Fixed some edge cases with dynamic SQL

### 0.1.7 (2021-01-05)

* Add `buildValue` parameter config to work around some dynamic SQL limitations

### 0.1.6 (2021-01-04)

* Fixed parameter and column inheritance

### 0.1.5 (2021-01-04)

* Fixed ignored columns in table DTOs

### 0.1.4 (2021-01-04)

* Now parses only included stored procedures
* Fixed rare script parsing error
* Fixed script base path bug

### 0.1.3 (2021-01-04)

* Support temp tables for scripts (thanks [@davidtme](davidtme)!) ([#2](https://github.com/cmeeren/Facil/pull/2), [#3](https://github.com/cmeeren/Facil/issues/3))
* Added ability to configure script base path
* Added ability to ignore columns in scripts, procedures, and table DTOs [#4](https://github.com/cmeeren/Facil/issues/4)

### 0.1.2 (2020-12-17)

* Added package logo

### 0.1.1 (2020-12-17)

* Improved error message for scripts with undeclared parameters used in EXEC

### 0.1.0 (2020-12-16)

* Initial release
