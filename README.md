# DotNetTestNSpec

[![NuGet Version and Downloads count](https://buildstats.info/nuget/dotnet-test-nspec)](https://www.nuget.org/packages/dotnet-test-nspec) 
[![Build status](https://ci.appveyor.com/api/projects/status/avtd9ca8mcuj4u6x/branch/master?svg=true)](https://ci.appveyor.com/project/BrainCrumbz/dotnettestnspec/branch/master)

DotNetTestNSpec is a NSpec runner for .NET Core command line interface and Visual Studio
IDE.

It runs NSpec tests in .NET Core projects targeting both .NET Core and .NET Framework,
both from console - taking advantage of *dotnet test* command line interface - as well as
from Visual Studio Test Explorer window.

For more info on NSpec testing framework, please see
[its project](https://github.com/nspec/NSpec) or [nspec.org](http://nspec.org/) website.

## Minimum requirements

It currently supports projects based on .NET Core Tools Preview 2, the ones with 
`project.json` and `.xproj` files, hence the only allowed Visual Studio IDE is 2015.

## Examples

See [NSpec](https://github.com/nspec/NSpec) project, at path
[examples/DotNetTestSample](https://github.com/nspec/NSpec/tree/master/sln/test/Samples/DotNetTestSample),
for a standalone solution with following features:

  * separate main and test project, 
  * import of NSpec and this runner as dependencies, 
  * allows running tests from *dotnet* command line interface and Visual Studio IDE.

## Breaking changes

To check for potential breaking changes, see [BREAKING-CHANGES.md](./BREAKING-CHANGES.md).

## Contributing

See [contributing](CONTRIBUTING.md) doc page.

## License

[MIT](./license.txt)

## Credits

DotNetTestNSpec is written by [BrainCrumbz](http://www.braincrumbz.com). It's shaped and
benefited by hard work from our [contributors](https://github.com/nspec/DotNetTestNSpec/contributors).
