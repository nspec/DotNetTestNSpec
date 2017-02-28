# Breaking Changes

## 1.0.0

### NSpec

Removed support for NSpec 2.0.1. Minimum NSpec required version is now 3.0.0.

#### Reason

Until now, *dotnet-test-nspec* and *NSpec* only supported .NET Core command line interface.
While evolving to integrate with VS 2015 IDE, inner API between the two changed and
broke compatibility.

#### Workaround

None. Remain on dotnet-test-nspec 0.1.1 if you need to support NSpec 2.0.1 test projects.
