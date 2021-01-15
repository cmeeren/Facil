Release notes
==============

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

