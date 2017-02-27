﻿using DotNetTestNSpec.Parsing;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Tests.Parsing
{
    [TestFixture]
    [Category("NSpecArgumentParser")]
    public abstract class describe_NSpecArgumentParser
    {
        protected NSpecArgumentParser parser;

        protected NSpecCommandLineOptions actual = null;

        protected string[] allArguments =
        {
            someClassName,
            "--tag", someTags,
            "--failfast",
            "--formatter=" + someFormatterName,
            "--formatterOptions:optName1=optValue1",
            "--formatterOptions:optName2",
            "--formatterOptions:optName3=optValue3",
            "--debugChannel",
            "--debugTests", someTestNamesArg,
        };

        protected NSpecCommandLineOptions allOptions;

        protected readonly string[] someTestNames =
        {
            "test Name 1",
            "test Name 2",
            "test Name 3",
        };

        protected const string someClassName = "someClassName";
        protected const string someTags = "tag1,tag2,tag3";
        protected const string someFormatterName = "someFormatterName";
        protected const string someTestNamesArg = "test Name 1, test Name 2, test Name 3";

        [SetUp]
        public virtual void setup()
        {
            allOptions = new NSpecCommandLineOptions()
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

            parser = new NSpecArgumentParser();
        }
    }

    public class when_only_nspec_args_found : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments;

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_nspec_opts_only()
        {
            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_tags_arg_missing : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--tag" && arg != someTags)
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_tags_null()
        {
            allOptions.Tags = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_tags_arg_incomplete : describe_NSpecArgumentParser
    {
        string[] args = null;

        public override void setup()
        {
            base.setup();

            args = allArguments
                .Where(arg => arg != someTags)
                .ToArray();
        }

        [Test]
        public void it_should_throw()
        {
            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }
    }

    public class when_failfast_arg_missing : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--failfast")
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_failfast_false()
        {
            allOptions.FailFast = false;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_formatter_arg_missing : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => !arg.StartsWith("--formatter=", StringComparison.Ordinal))
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_formatter_null()
        {
            allOptions.FormatterName = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_formatter_options_arg_missing : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => !arg.StartsWith("--formatterOptions:", StringComparison.Ordinal))
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_formatter_options_empty()
        {
            allOptions.FormatterOptions = new Dictionary<string, string>();

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_debug_channel_arg_missing : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--debugChannel")
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_debug_channel_false()
        {
            allOptions.DebugChannel = false;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_debug_tests_arg_missing : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != "--debugTests" && arg != someTestNamesArg)
                .ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_opts_with_debug_tests_null()
        {
            allOptions.DebugTests = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_debug_tests_arg_incomplete : describe_NSpecArgumentParser
    {
        string[] args = null;

        public override void setup()
        {
            base.setup();

            args = allArguments
                .Where(arg => arg != someTestNamesArg)
                .ToArray();
        }

        [Test]
        public void it_should_throw()
        {
            Assert.Throws<ArgumentException>(() => parser.Parse(args));
        }
    }

    public class when_unknown_args_found_after_class_name : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            var argList = allArguments.ToList();

            argList.Insert(1, "unknown1");
            argList.Insert(5, "unknown2");
            argList.Add("unknown3");

            string[] args = argList.ToArray();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_nspec_opts_with_unknown_args()
        {
            allOptions.UnknownArgs = new string[]
            {
                "unknown1",
                "unknown2",
                "unknown3",
            };

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_class_name_arg_missing : describe_NSpecArgumentParser
    {
        readonly string[] keysWithFurtherArgs =
        {
            "--tag",
            "--debugTests",
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
        public void it_should_return_opts_with_null_class_name(string[] args)
        {
            actual = parser.Parse(args);

            allOptions.ClassName = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }

        bool SequenceIsAllowed(Queue<string> queue)
        {
            bool lastArgumentNeedsFurtherOnes =
                keysWithFurtherArgs.Contains(queue.Last());

            return !lastArgumentNeedsFurtherOnes;
        }
    }

    public class when_nspec_parser_args_empty : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = new string[0];

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_nspec_opts_empty()
        {
            var expected = new NSpecCommandLineOptions()
            {
                FormatterOptions = new Dictionary<string, string>(),
                UnknownArgs = new string[0],
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
