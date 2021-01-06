Release notes
==============

### vNext

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

