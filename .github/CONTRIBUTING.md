Contributor guidelines
======================

First of all – thanks for taking the time to contribute!

We welcome the contributions from non-members. That said, we’d like to do things right rather than fast. To make everyone's experience as enjoyable as possible, read [Don't "Push" Your Pull Requests](https://www.igvita.com/2011/12/19/dont-push-your-pull-requests/) and please keep the following things in mind:

- Unless it's a trivial fix, consider opening an issue first to discuss it with the team.
- For all pull requests, please follow the workflow described below.

Opening an issue
----------------

- Before opening an issue, please check if there's a known workaround, existing issue, or already a work in progress to address it.

- Provide as much relevant info as possible. If there’s an issue template, follow it if it makes sense.

- If you are requesting a feature, it is best if you spend the time to think it through:

  - Which use-cases does it serve?
  - Can the uses-cases be facilitated in another way?
  - How could the API be designed to support the use-cases, and what are the tradeoffs to each approach?

  Even if the points may seem clear, saying something about them in your issue makes it clear that you have spent at least a minimal amount of effort thinking about the ramifications of this change.

Creating a pull request
-----------------------

(Based on https://github.com/App-vNext/Polly/wiki/Git-Workflow)

To contribute to Facil while ensuring a smooth experience for all involved, please ensure you follow all of these steps:

1. Fork Facil on GitHub
2. Clone your fork locally
3. Add the upstream repo: `git remote add upstream git@github.com:cmeeren/Facil.git`
4. Create a local branch: `git checkout -b myBranch`
5. Work on your feature
6. Rebase if required (see below)
7. Push the branch up to GitHub: `git push origin myBranch`
8. Send a Pull Request on GitHub

You should **never** work on a clone of master, and you should **never** send a pull request from master - always from a branch. The reasons for this are detailed below.

### Rebasing when handling updates from `upstream/master`

While you're working on your branch it's quite possible that your upstream master may be updated. If this happens you should:

1. [Stash](https://git-scm.com/book/en/v2/Git-Tools-Stashing-and-Cleaning) any un-committed changes you need to
2. `git checkout master`
3. `git pull upstream master`
4. `git rebase master myBranch`
5.  `git push origin master` (optional; this this makes sure your remote master is up to date)

This ensures that your history is “clean”, with one branch off from master containing your changes in a straight line. Failing to do this ends up with several messy merges in your history, which we’d rather avoid in order to keep the project history understandable. This is the reason why you should always work in a branch and you should never be working in, or sending pull requests from, `master`.

If you have pushed your branch to GitHub and you need to rebase like this (including after you have created a pull request), you need to use `git push -f` to force rewrite the remote branch.

Also consider cleaning your commit history by squashing commits in an interactive rebase (not mandatory).

More on rebasing and squashing can be found in [this guide](https://robots.thoughtbot.com/git-interactive-rebase-squash-amend-rewriting-history).

Development
-----------

#### Dev dependencies

* A SQL Server (e.g. Developer Edition) instance with full-text search available at `Data Source=.`
  * If you’d like to use another SQL Server instance, just modify the data source in `DbTests.DbGen\appsettings.json` and `TestDb\LocalDB.publish.xml`
* Publish using `TestDb\LocalDB.publish.xml` (in VS, just double-click it and choose Publish)

#### Solution structure

* `Facil.Generator` is the generator console app. The build output is copied as-is into the nupkg as a build tool.
* `Facil.Runtime` and `Facil.Runtime.CSharp` contain runtime utilities used by the generated code.
* ` Facil.Package` is the project that pulls together the generator, build tasks and runtime components into a single package, and also specifies all the package dependencies. It does not contain any code itself.
* `DbTests` is the unit test project.
* `DbTests.DbGen` is the generation output project referenced by `DbTests`. Generation output is in a separate project because it takes a while to recompile due to the amount of generated code, so having it separated from the tests means the test project can be modified and recompiled more quickly. In order to also test the build task, this project references Facil using `PackageReference`, which after building `Facil.Package` exists in and (due to the solution’s `nuget.config`) is restored from the `nupkg` directory in the solution root.
* `TestDb` is the database project that contains the schema used by `DbTests.DbGen`.

#### Dev workflow: Quick testing of generator output:

* Set `Facil.Generator` as the startup project
* Run it when needed and verify the output in the `DbTests.DbGen` project
* Remember to perform the steps below to properly update the test project

#### Dev workflow: Updating test project and running tests

After making changes in the generator, runtime, or package projects:

* Set `DbTests` as the startup project
* Right-click `Facil.Package` and choose Build
* Right-click `Facil.Package` and choose Pack
* Right-click `DbTests.DbGen` and choose Rebuild (not Build)
* Run the test project as a normal console app (orders of magnitude faster than using the test explorer, and there seems to be some issues where tests lock up in the Visual Studio Test Explorer)

Notes about this workflow:

* Build before Pack is needed due to what looks like some timing issues where Pack alone doesn’t (always) pick up the most recent generator/runtime files (this includes using `<GeneratePackageOnBuild>`).
* `Facil.Package` can’t have a project reference to `Facil.Generator` due to the different target frameworks, but the solution file has a build dependency set up between them, so that building `Facil.Package` will also build `Facil.Generator`. If using command-line utilities (like in the AppVeyor build script), the generator must be built manually before `Facil.Package`.
* The Pack step will also remove the cached Facil version in the solution’s NuGet package cache (as well as all existing nupkg files in the nupkg output folder), ensuring that when `DbTests` is rebuilt, it restores the most recently built package.

## Deployment checklist

For maintainers.

* Make necessary changes to the code
* Update the changelog
* Update the version in `Directory.Build.props`
* Regenerate the test project (see steps above)
* Commit and tag the commit in the format `v/x.y.z` (this is what triggers deployment from AppVeyor)
* Push the changes and the tag to the repo. If the AppVeyor build succeeds, the package is automatically published to NuGet.
