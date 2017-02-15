using DotNetTestNSpec.Parsing;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetTestNSpec.Tests.Parsing
{
    [TestFixture]
    [Category("ArgumentParser")]
    public class describe_ArgumentParser
    {
        protected ArgumentParser parser;

        protected CommandLineOptions actual = null;

        protected CommandLineOptions allOptions;

        protected readonly string[] allArguments =
        {
            Data.DotNet.someProjectPath,
            "--designtime",
            "--list",
            "unknown-dotnet-1",
            "--wait-command",
            "--parentProcessId", Data.DotNet.someProcessIdArg,
            "--port", Data.DotNet.somePortArg,
            "unknown-dotnet-2",

            "--",

            Data.NSpec.someClassName,
            "--tag", Data.NSpec.someTags,
            "--failfast",
            "--formatter=" + Data.NSpec.someFormatterName,
            "unknown-nspec-1",
            "--formatterOptions:optName1=optValue1",
            "--formatterOptions:optName2",
            "--formatterOptions:optName3=optValue3",
            "--debugChannel",
            "--debugTests", Data.NSpec.someTestNamesArg,
            "unknown-nspec-2",
        };

        protected readonly string[] someUnknownArgs =
        {
            "unknown-dotnet-1",
            "unknown-dotnet-2",
            "unknown-nspec-1",
            "unknown-nspec-2",
        };

        [SetUp]
        public void setup()
        {
            var allDotNetOptions = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            var allNSpecOptions = new NSpecCommandLineOptions(Data.NSpec.allOptions);

            allOptions = new CommandLineOptions()
            {
                DotNet = allDotNetOptions,
                NSpec = allNSpecOptions,
                UnknownArgs = someUnknownArgs,
            };

            parser = new ArgumentParser();

            actual = parser.Parse(allArguments);
        }

        [Test]
        public void it_should_return_all_opts_with_unknown_args()
        {
            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
