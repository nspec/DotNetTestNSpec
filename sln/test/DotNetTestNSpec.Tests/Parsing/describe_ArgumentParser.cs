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
            someProjectPath,
            "--designtime",
            "--list",
            "unknown-dotnet-1",
            "--wait-command",
            "--parentProcessId", "123",
            "--port", "456",
            "unknown-dotnet-2",

            "--",

            someClassName,
            "--tag", someTags,
            "--failfast",
            "--formatter=" + someFormatterName,
            "unknown-nspec-1",
            "--formatterOptions:optName1=optValue1",
            "--formatterOptions:optName2",
            "--formatterOptions:optName3=optValue3",
            "--debugChannel",
            "--debugTests", someTestNamesArg,
            "unknown-nspec-2",
        };

        protected readonly string[] someTestNames =
        {
            "test Name 1",
            "test Name 2",
            "test Name 3",
        };

        protected readonly string[] someUnknownArgs =
        {
            "unknown-dotnet-1",
            "unknown-dotnet-2",
            "unknown-nspec-1",
            "unknown-nspec-2",
        };

        protected const string someProjectPath = @"Path\To\Some\Project";
        protected const string someClassName = "someClassName";
        protected const string someTags = "tag1,tag2,tag3";
        protected const string someFormatterName = "someFormatterName";
        protected const string someTestNamesArg = "test Name 1, test Name 2, test Name 3";

        [SetUp]
        public void setup()
        {
            var allDotNetOptions = new DotNetCommandLineOptions()
            {
                Project = someProjectPath,
                DesignTime = true,
                List = true,
                WaitCommand = true,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            var allNSpecOptions = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = someTags,
                FailFast = true,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                DebugChannel = true,
                DebugTests = someTestNames,
                UnknownArgs = new string[0],
            };

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
