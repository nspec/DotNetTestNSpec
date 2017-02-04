# Contributing

The DotNetTestNSpec test suite is written in NUnit. The test project is DotNetTestNSpec.Tests.

To build and run tests from command line, use .NET Core `dotnet` command line interface

```dos
cd path\to\DotNetTestNSpec\sln
dotnet restore
cd test\DotNetTestNSpec.Tests
dotnet test
```

Alternatively, to run tests, use your test runner of choice in Visual Studio TDD.Net, NCrunch, Resharper's Test Runner, et al)

Fork the project, make your changes, and then send a Pull Request.

## Branch housekeeping

If you are a direct contributor to the project, please keep an eye on your past development or features branches and think about archiving them once they're no longer needed. 
No worries, their commits will still be available under named tags, it's just that they will not pollute the branch list.

If you're running on a Windows OS, there's a batch script available at `scripts\archive-branch.bat`. Otherwise, the command sequence to run in a *nix shell is the following:

```dos
# Get local branch from remote, if needed
git checkout <your-branch-name>

# Go back to master
git checkout master

# Create local tag
git tag archive/<your-branch-name> <your-branch-name>

# Create remote tag
git push origin archive/<your-branch-name>

# Delete local branch
git branch -d <your-branch-name>

# Delete remote branch
git push origin --delete <your-branch-name>
```

If you need to later retrieve an archived branch, just run the following commands:

```dos
# Checkout archive tag
git checkout archive/<your-branch-name>

# (Re)Create branch
git checkout -b <some-branch-name>
```
