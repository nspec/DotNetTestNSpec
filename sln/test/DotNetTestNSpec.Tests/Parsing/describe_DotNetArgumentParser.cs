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
            Data.DotNet.someProjectPath,
            "--designtime",
            "--list",
            "--wait-command",
            "--parentProcessId", Data.DotNet.someProcessIdArg,
            "--port", Data.DotNet.somePortArg,
        };

        [SetUp]
        public virtual void setup()
        {
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
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

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
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            expected.DesignTime = false;

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
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            expected.List = false;

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
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            expected.WaitCommand = false;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_dotnet_test_parent_process_id_arg_missing : describe_DotNetArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--parentProcessId" && arg != Data.DotNet.someProcessIdArg)
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_parent_process_id_null()
        {
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            expected.ParentProcessId = null;

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
                .Where(arg => arg != Data.DotNet.someProcessIdArg)
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
                .Where(arg => arg != "--port" && arg != Data.DotNet.somePortArg)
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_port_null()
        {
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            expected.Port = null;

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
                .Where(arg => arg != Data.DotNet.somePortArg)
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
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            expected.NSpecArgs = new string[]
            {
                "SomeClassName",
                "--tag",
                "tag1,tag2,tag3",
            };

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
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

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
                Data.DotNet.someProjectPath,
                "unknown1",
                "--parentProcessId", Data.DotNet.someProcessIdArg,
                "--port", Data.DotNet.somePortArg,
                "unknown2",
            };

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_dotnet_test_opts_with_unknown_args()
        {
            var expected = new DotNetCommandLineOptions()
            {
                Project = Data.DotNet.someProjectPath,
                ParentProcessId = Data.DotNet.someProcessId,
                Port = Data.DotNet.somePort,
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
            var expected = new DotNetCommandLineOptions(Data.DotNet.allOptions);

            expected.Project = null;

            actual = parser.Parse(args);

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
