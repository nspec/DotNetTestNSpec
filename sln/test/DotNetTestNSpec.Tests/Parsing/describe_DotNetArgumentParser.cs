using DotNetTestNSpec.Parsing;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Tests.Parsing
{
    [TestFixture]
    [Category("DotNetArgumentParser")]
    public abstract class describe_DotNetArgumentParser
    {
        protected DotNetArgumentParser parser;

        protected DotNetCommandLineOptions actual = null;

        protected readonly string[] allArguments =
        {
            someProjectPath,
            "--designtime",
            "--list",
            "--wait-command",
            "--parentProcessId", "123",
            "--port", "456",
        };

        protected DotNetCommandLineOptions allOptions;

        protected const string someProjectPath = @"Path\To\Some\Project";

        [SetUp]
        public virtual void setup()
        {
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

            parser = new DotNetArgumentParser();
        }
    }

    public class when_only_dotnet_test_args_found : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments;

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_opts_only()
        {
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

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_design_time_false()
        {
            allOptions.DesignTime = false;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_list_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--list")
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_list_false()
        {
            allOptions.List = false;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_wait_command_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--wait-command")
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_wait_command_false()
        {
            allOptions.WaitCommand = false;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_parent_process_id_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--parentProcessId" && arg != "123")
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_parent_process_id_null()
        {
            allOptions.ParentProcessId = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_parent_process_id_arg_incomplete : describe_DotNetArgumentParser
    {
        string[] args = null;

        public override void setup()
        {
            base.setup();

            args = allArguments
                .Where(arg => arg != "123")
                .ToArray();
        }

        [Test]
        public void it_should_throw()
        {
            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }
    }

    public class when_dotnet_test_port_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--port" && arg != "456")
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_port_null()
        {
            allOptions.Port = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_port_arg_incomplete : describe_DotNetArgumentParser
    {
        string[] args = null;

        public override void setup()
        {
            base.setup();

            args = allArguments
                .Where(arg => arg != "456")
                .ToArray();
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

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_opts_with_nspec_args()
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

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_opts_only()
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

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_opts_with_unknown_args()
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

    public class when_dotnet_test_project_arg_missing : describe_DotNetArgumentParser
    {
        readonly string[] valuedArguments =
        {
            "--parentProcessId",
            "--port",
        };

        [DatapointSource]
        public IEnumerable<string[]> ArgumentRotations
        {
            get
            {
                var queue = new Queue<string>(allArguments.Skip(1));

                for (int i = 0; i < queue.Count; i++)
                {
                    if (SequenceIsAllowed(queue))
                    {
                        yield return queue.ToArray();
                    }

                    queue.Enqueue(queue.Dequeue());
                }
            }
        }

        [Theory]
        public void it_should_return_opts_with_project_null(string[] args)
        {
            actual = parser.Parse(args);

            allOptions.Project = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }

        bool SequenceIsAllowed(Queue<string> queue)
        {
            bool lastArgumentNeedsValue =
                valuedArguments.Contains(queue.Last());

            return !lastArgumentNeedsValue;
        }
    }

    public class when_dotnet_parser_args_empty : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = new string[0];

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_opts_empty()
        {
            var expected = new DotNetCommandLineOptions()
            {
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_only_separator_arg_found : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args =
            {
                "--",
            };

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_opts_empty()
        {
            var expected = new DotNetCommandLineOptions()
            {
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
