using DotNetTestNSpec.Parsing;
using FluentAssertions;
using NUnit.Framework;
using System;

namespace DotNetTestNSpec.Tests.Parsing
{
    public abstract class describe_DotNetArgumentParser
    {
        protected DotNetCommandLineOptions actual = null;

        protected const string projectValue = @"Path\To\Some\Project";
    }

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_only_dotnet_test_args_found : describe_DotNetArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--designtime",
                "--parentProcessId", "123",
                "--port", "456",
            };

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_args_only()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = projectValue,
                DesignTime = true,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_dotnet_test_project_arg_missing : describe_DotNetArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                "--parentProcessId", "123",
                "--port", "456",
            };

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_project()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = null,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_dotnet_test_design_time_arg_missing : describe_DotNetArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--parentProcessId", "123",
                "--port", "456",
            };

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_project()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = projectValue,
                DesignTime = false,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_some_dotnet_test_arg_missing : describe_DotNetArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
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
                Project = projectValue,
                ParentProcessId = 123,
                Port = null,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_dotnet_test_arg_value_missing : describe_DotNetArgumentParser
    {
        DotNetArgumentParser parser = null;
        string[] args = null;

        [SetUp]
        public void setup()
        {
            args = new string[]
            {
                projectValue,
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

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_dotnet_test_and_nspec_args_found : describe_DotNetArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--parentProcessId", "123",
                "--port", "456",
                "--",
                "SomeClassName",
                "--tag",
                "tag1,tag2,tag3",
            };

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_and_nspec_args()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = projectValue,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[]
                {
                    "SomeClassName",
                    "--tag",
                    "tag1,tag2,tag3",
                },
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_no_nspec_args_found_after_separator : describe_DotNetArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
                "--parentProcessId", "123",
                "--port", "456",
                "--",
            };

            var parser = new DotNetArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_args_only()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = projectValue,
                ParentProcessId = 123,
                Port = 456,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    [TestFixture]
    [Category("DotNetArgumentParser")]
    public class when_unknown_args_found_before_separator : describe_DotNetArgumentParser
    {
        [SetUp]
        public void setup()
        {
            string[] args =
            {
                projectValue,
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
                Project = projectValue,
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
