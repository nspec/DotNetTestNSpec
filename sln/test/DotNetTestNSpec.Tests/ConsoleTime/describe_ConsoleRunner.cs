using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.Proxy;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DotNetTestNSpec.Tests.ConsoleTime
{
    public abstract class describe_ConsoleRunner
    {
        protected Mock<IControllerProxy> controller;
        protected CommandLineOptions opts;
        protected ConsoleRunner runner;

        protected readonly CommandLineOptions.NSpecPart nspecOptions = new CommandLineOptions.NSpecPart()
        {
            Tags = "tag1,tag2,tag3",
            FormatterName = @"someFormatterName",
            FormatterOptions = new Dictionary<string, string>()
                    {
                        { "optName1", "optValue1" },
                        { "optName2", "optName2" },
                        { "optName3", "optValue3" },
                    },
            FailFast = true,
        };

        protected const string testAssemblyPath = @"some\path\to\assembly";

        [SetUp]
        public void setup()
        {
            controller = new Mock<IControllerProxy>();

            runner = new ConsoleRunner(opts, controller.Object);
        }
    }

    [TestFixture]
    [Category("ConsoleRunner")]
    public class when_started : describe_ConsoleRunner
    {
        public when_started()
        {
            opts = new CommandLineOptions()
            {
                DotNet = new CommandLineOptions.DotNetPart()
                {
                    Project = testAssemblyPath,
                },
                NSpec = nspecOptions,
            };
        }

        [Test]
        public void it_should_return_nr_of_failures()
        {
            int expected = 123;

            controller.Setup(c => c.Run(
                testAssemblyPath,
                opts.NSpec.Tags,
                opts.NSpec.FormatterName,
                opts.NSpec.FormatterOptions,
                opts.NSpec.FailFast)).Returns(expected);

            int actual = runner.Start();

            actual.Should().Be(expected);
        }
    }
}
