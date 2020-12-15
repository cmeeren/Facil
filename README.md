Facil
==============

**Boilerplate-free, strongly typed SQL without the drawbacks of type providers. Optimized for developer happiness.**

### Elevator pitch

Facil is a friendly, flexible, and full-featured fabricator of files for fluent and frictionless facades facilitating the facile and functional fetching of facts.

(“Facts” referring to data stored in SQL server. It would be better if SQL started with an F. Oh well.)

Okay, elevator pitch without the alliteration: Facil works similarly to type providers like [FSharp.Data.SqlClient](https://github.com/fsprojects/FSharp.Data.SqlClient/) by letting you call SQL scripts and stored procedures in a strongly typed manner, but it avoids a range of type provider issues and hiccups by not being a type provider and actually generating F# code that you check in. (Why not a type provider, you ask? See the FAQ.)

Core features:

* Primary goal: Provide the simplest way (yet highly configurable) to call stored procedures and SQL scripts as if they were F# functions, and otherwise get out of your way and let you get on with providing actual business value
* Good API ergonomics – succinct and discoverable fluent-style syntax, no boilerplate
* Supports SQL Server 2012 and up
* Built for the future, compatible with .NET Standard 2.0
* Thoroughly tested
* Built for speed – inner async read loops written in C# using its native `async`/`await`; allows you to read directly to your chosen DTO records to minimize allocations for heavy queries
* Highly configurable with simple, yet flexible [YAML configuration](https://github.com/cmeeren/Facil/blob/master/facil_reference.yaml)
* Helpful build-time error messages and warnings
* At runtime, supply a connection string for simplicity or use your own managed connections
* Can be configured to use `ValueOption` instead of `Option` (potentially specific to single procedure/script inputs and outputs)
* Can create DTOs for tables and automatically (or manually) use those DTOs as return values for matching result sets, simplifying your mapping to domain entities
* Can also map directly to any record type you specify
* Can accept suitable DTOs instead of a list of parameters, e.g. you can just pass your `UserDto` to your `SaveUser` procedure instead of explicitly supplying all parameters from the DTO
* Supports table-valued parameters in both procedures and scripts
* Supports stored procedure output parameters and return values
* Supports lazy execution, both sync (returns `seq`) and async (if your target supports .NET Standard 2.1 – returns `IAsyncEnumerable`, use with e.g. [FSharp.Control.AsyncSeq](https://github.com/fsprojects/FSharp.Control.AsyncSeq))

### Production readiness

Facil is production ready.

Facil contains almost 2000 tests verifying most functionality in many different combinations, and will soon be used in several mission-critical production services at our company. I’m not claiming it’s perfect, or even bug-free, but it’s well tested, and I have a vested interest in keeping it working properly.

It’s still at 0.x because it's still new and I may still be discovering improvements that require breaking changes every now and then. However, do not take 0.x to mean that it’s a buggy mess, or that the API will radically change every other week. Breaking changes will cause a lot of churn for me, too.

### A note on versioning

While at 0.x, I’ll try to increment the minor version for breaking changes and the patch version for anything else.

Note on what a “breaking change” is: There is a lot of the generated code that needs to be public to support inlining, but that is still considered implementation details. These parts of the API are hidden from the IDE using the `[<EditorBrowsable>]` attribute to ensure you won’t use them by accident, but there’s nothing stopping you from looking at the generated code and referencing these parts of the API in your code. Don’t do that. These are implementation details and may change at any time.

Contributing
------------

Contributions and ideas are welcome! Please see [Contributing.md](https://github.com/cmeeren/Facil/blob/master/.github/CONTRIBUTING.md) for details.

Quick start
-----------

#### 0. Requirements

* SQL Server 2012 at build-time (may work with older versions at runtime; untested)
* .NET Core 3.1 or later at build-time

#### 1. Install

Install Facil from [NuGet](https://www.nuget.org/packages/Facil).

#### 2. Build and edit the new config file

Start a build. It will fail. Facil will place [a minimal config](https://github.com/cmeeren/Facil/blob/master/src/Facil.Generator/facil_minimal.yaml) file in your project directory. Edit it – you should at least set a connection string. For more details, you can find [the full config reference here](https://github.com/cmeeren/Facil/blob/master/facil_reference.yaml). You might consider adding the config file to your project for easy access in Visual Studio.

#### 3. Build again and add the generated file

Start a new build. Facil will now generate the code. Add the generated file to your project.

#### 4. Use the generated code

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
        .WithParameters(dtoWithPrimitiveParams)
        .AsyncExecute()
	}
```

#### 5. Profit!

That’s it! For regenerating, see the FAQ entry “When does Facil regenerate files?”.

FAQ
---

#### Why not a type provider?

Type providers are great in theory, and to a large extent also in practice, but have some notable drawbacks:

* Potentially horrible IDE performance if you use it a lot, killing your productivity
* No standardized schema caching or “offline” mode where it will use the already generated types without hitting your DB; any offline implementation is entirely up to the type provider implementer, meaning support may be hit-and-miss or completely missing 
* Not necessarily CI/CD friendly; you may be required to ensure that the build server has access to a suitable database, either destroying repeatable/parallel builds (if using an external database) or placing cumbersome constraints on the build agents (if using a DB on the build agent), and in all cases lengthening the build time
* Limitations for what kind of types can be provided (no union types being a well-known example, though not itself relevant for Facil)
* May need to reload projects to force the TP to update after changes to external schema

Facil, by generating plain old F# code (that you check in and don’t have to maintain), sidesteps all of these issues.

#### When does Facil regenerate files?

Facil will regenerate (hitting your DB) on the next build after:

* Changes to the two first lines of the generated file(s) (the simplest way to manually force a rebuild)
* Changes in included SQL scripts
* Changes in effective config (including variable contents)
* Changes to Facil itself (i.e., when updating Facil)

When there are no changes as described above, Facil will skip its build step and thus not hit your DB.

Notably, this means that Facil **will not pick up changes to your database**. This is by design. If you update the DB that Facil connects to during build, just open the generated file and delete the first line and Facil will re-generate it on the next build.

#### Can I force Facil to run during CI build and fail if the generated file is not up to date?

Yes. There are two environment variables you can set. You can use either of them on their own, or together:

* `FACIL_FORCE_REGENERATE`: Set this to force Facil to always regenerate during build. Alone, this variable will effectively make Facil mimick a type provider without caching/offline capabilities.
* `FACIL_FAIL_ON_CHANGED_OUTPUT`: Set this to make Facil fail the build if the output has changed. You can use this to reject commits that does not include up-to-date generated code.

#### What can I configure?

See [the full YAML config reference for details](https://github.com/cmeeren/Facil/blob/master/facil_reference.yaml). Note that you can generate multiple source files with separate configs (e.g. to generate from multiple DBs). Here are some highlights of what you can configure.

For each file, you can configure:

* The generated filename
* The generated namespace/module
* Arbitrary code to put that the top of the generated file
* Which stored procedures (regex matching) or scripts (glob matching) to generate code for
* Which tables to generate DTO records for (which can be used, automatically or manually, for matching procedure/script result sets)

For each procedure/script (or any set that matches a specified pattern), you can configure:

* The result type: Anonymous record, auto-pick best table DTO (with fallback to anonymous records), or any record type you specify (a table DTO or your own custom type) that is constructible by the generated code
* Whether to use `ValueOption` instead of `Option` for inputs and/or outputs
* For single-column results, whether to return a record (as with multiple columns) or just return the scalar value
* Whether to skip the `inline` DTO parameter overloads (for faster compilation if you don’t use them)
* Whether to use return values (stored procedures only)
* For each stored procedure parameter: The name to use in parameter DTO objects
* For each script parameter: The name to use in parameter DTO objects, as well as its type and nullability (to work around type inference limitations for scripts, see below)

For each table DTO, you can configure:

* Whether to use `ValueOption` instead of `Option`

For each table type (automatically included by Facil if used in included procedures/scripts), you can configure:

* Whether to use `ValueOption` instead of `Option`
* Whether to skip the `inline` DTO parameter overloads (for faster compilation if you don’t use them)

#### Type inference limitations in scripts

Type inference in scripts is limited due to limitations in SQL Server's `sp_describe_undeclared_parameters`, which Facil uses to get parameter information for scripts. Notably, the following does not work out of the box:

* Parameters used multiple times will give errors
* Table-valued parameters will give errors
* Nullability is not inferred (by default, Facil assumes all script parameters are non-nullable)

To work around this, for each problematic parameter (you don't have to specify the ones that work), you can specify in the config which SQL type the parameter is and whether it is nullable. You can also override all parameters at once.

#### How is default and nullable parameter values handled?

All stored procedure parameters that have `null` as the default value are treated as nullable and wrapped in `option` (or `voption`). All other default values for stored procedure parameters are ignored; the parameters will be required and non-nullable.

While parameters with default values could conceivably be generated as optional method parameters, this runs the risk of forgetting to use them when executing the procedure (I’ve been burned by this a lot), and it also won’t mesh that well with the parameter DTO overloads.

If Facil’s current approach does not work for you, please open an issue and describe your use-case.

#### Can you support user-defined functions?

If you need this, I’m willing to hear you out, but this isn’t high on my priority list right now. A simple workaround is to simply call the function from a script or stored procedure, and then use Facil with that script/procedure instead.

#### Can Facil generate SQL, too?

No, this is not currently supported and not planned. While it may sound useful to generate boilerplate scripts for “select from table by primary key” or “update/insert/merge table”, there are in my experience enough considerations and slight variants of these patterns to take into account that I’m not convinced it would be terribly useful. Facil is focused on allowing you to call your existing TSQL in the simplest fashion possible; it won’t generate TSQL For you.

Release notes
-------------

[RELEASE_NOTES.md](https://github.com/cmeeren/Facil/blob/master/RELEASE_NOTES.md)

