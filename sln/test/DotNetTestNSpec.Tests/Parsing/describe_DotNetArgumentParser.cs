using DotNetTestNSpec.Parsing;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace DotNetTestNSpec.Tests.Parsing
{
    [TestFixture]
    [Category("DotNetArgumentParser")]
    public abstract class describe_DotNetArgumentParser
    {
        protected DotNetCommandLineOptions actual = null;

        protected string[] allArguments;
        protected DotNetCommandLineOptions allOptions;

        protected const string someProjectPath = @"Path\To\Some\Project";

        [SetUp]
        public virtual void setup()
        {
            allArguments = new string[]
            {
                someProjectPath,
                "--designtime",
                "--list",
                "--wait-command",
                "--parentProcessId", "123",
                "--port", "456",
            };

            allOptions = new DotNetCommandLineOptions()
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
        }
    }

    public class when_only_dotnet_test_args_found : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments;

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_args_only()
        {
            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_project_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != someProjectPath)
                .ToArray();

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_project()
        {
            allOptions.Project = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_design_time_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--designtime")
                .ToArray();

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_design_time_false()
        {
            allOptions.DesignTime = false;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_some_dotnet_test_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args =
            {
                someProjectPath,
                "--parentProcessId", "123",
            };

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_found_args_only()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = someProjectPath,
                ParentProcessId = 123,
                Port = null,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_arg_value_missing : describe_DotNetArgumentParser
    {
        DotNetArgumentParser parser = null;
        string[] args = null;

        public override void setup()
        {
            base.setup();

            args = new string[]
            {
                someProjectPath,
                "--parentProcessId", "123",
                "--port",
            };

            parser = new DotNetArgumentParser();
        }

        [Test]
        public void it_should_throw()
        {
            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }
    }

    public class when_dotnet_test_and_nspec_args_found : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] furtherArgs =
            {
                "--",
                "SomeClassName",
                "--tag",
                "tag1,tag2,tag3",
            };

            string[] args = allArguments
                .Concat(furtherArgs)
                .ToArray();

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_and_nspec_args()
        {
            allOptions.NSpecArgs = new string[]
            {
                "SomeClassName",
                "--tag",
                "tag1,tag2,tag3",
            };

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_no_nspec_args_found_after_separator : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] furtherArgs =
            {
                "--",
            };

            string[] args = allArguments
                .Concat(furtherArgs)
                .ToArray();

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_args_only()
        {
            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_unknown_args_found_before_separator : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args =
            {
                someProjectPath,
                "unknown1",
                "--parentProcessId", "123",
                "--port", "456",
                "unknown2",
            };

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_and_unknown_args()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = someProjectPath,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[]
                {
                    "unknown1",
                    "unknown2",
                },
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
