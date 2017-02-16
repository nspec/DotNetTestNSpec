using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.Proxy;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DotNetTestNSpec.Tests.ConsoleTime
{
    [TestFixture]
    [Category("ConsoleRunner")]
    public abstract class describe_ConsoleRunner
    {
        protected ConsoleRunner runner;

        protected Mock<IControllerProxy> controllerProxy;

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
        public virtual void setup()
        {
            controllerProxy = new Mock<IControllerProxy>();

            runner = new ConsoleRunner(testAssemblyPath, controllerProxy.Object, nspecOptions);
        }
    }

    public class when_started : describe_ConsoleRunner
    {
        const int nrOfFailures = 123;

        public override void setup()
        {
            base.setup();

            controllerProxy.Setup(c => c.Run(
                testAssemblyPath,
                nspecOptions.Tags,
                nspecOptions.FormatterName,
                nspecOptions.FormatterOptions,
                nspecOptions.FailFast)).Returns(nrOfFailures);
        }

        [Test]
        public void it_should_return_nr_of_failures()
        {
            int expected = nrOfFailures;

            int actual = runner.Start();

            actual.Should().Be(expected);
        }
    }
}
