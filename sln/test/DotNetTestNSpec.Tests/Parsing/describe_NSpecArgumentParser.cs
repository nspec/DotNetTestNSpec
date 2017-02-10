using DotNetTestNSpec.Parsing;
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
        protected NSpecCommandLineOptions actual = null;

        protected string[] allArguments;
        protected NSpecCommandLineOptions allOptions;

        protected const string someClassName = @"someClassName";
        protected const string someTags = "tag1,tag2,tag3";
        protected const string someFormatterName = @"someFormatterName";

        [SetUp]
        public virtual void setup()
        {
            allArguments = new[]
            {
                    someClassName,
                "--tag", someTags,
                "--failfast",
                "--formatter=" + someFormatterName,
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
                "--debugChannel",
            };

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
                UnknownArgs = new string[0],
            };
        }
    }

    public class when_only_nspec_args_found : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments;

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_nspec_args_only()
        {
            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_class_name_arg_missing : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args = allArguments
                .Where(arg => arg != someClassName)
                .ToArray();

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_class_name()
        {
            allOptions.ClassName = null;

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

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_tags()
        {
            allOptions.Tags = null;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
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

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_failfast_false()
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

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_null_formatter()
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

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_empty_formatter_options()
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

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_args_with_debug_channel_false()
        {
            allOptions.DebugChannel = false;

            var expected = allOptions;

            actual.ShouldBeEquivalentTo(expected);
        }
    }

    public class when_unknown_args_found_after_classname : describe_NSpecArgumentParser
    {
        public override void setup()
        {
            base.setup();

            string[] args =
            {
                someClassName,
                "unknown1",
                "--tag", someTags,
                "--failfast",
                "unknown2",
                "--formatter=" + someFormatterName,
                "--formatterOptions:optName1=optValue1",
                "--formatterOptions:optName2",
                "--formatterOptions:optName3=optValue3",
                "--debugChannel",
                "unknown3",
            };

            var parser = new NSpecArgumentParser();

            actual = parser.Parse(args);
        }

        [Test]
        public void it_should_return_nspec_and_unknown_args()
        {
            var expected = new NSpecCommandLineOptions()
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
                UnknownArgs = new string[]
                {
                    "unknown1",
                    "unknown2",
                    "unknown3",
                },
            };

            actual.ShouldBeEquivalentTo(expected);
        }
    }
}
