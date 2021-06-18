Release notes
==============

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

