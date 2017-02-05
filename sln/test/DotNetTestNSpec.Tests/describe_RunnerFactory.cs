using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Parsing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DotNetTestNSpec.Tests
{
    public abstract class describe_RunnerFactory
    {
        protected ITestRunner actual;

        protected CommandLineOptions opts;

        protected readonly string[] args = { "one", "two", "three" };

        [SetUp]
        public void setup()
        {
            var argumentParser = new Mock<IArgumentParser>();

            argumentParser.Setup(p => p.Parse(args)).Returns(opts);

            var factory = new RunnerFactory(argumentParser.Object);

            actual = factory.Create(args);
        }
    }

    [TestFixture]
    [Category("RunnerFactory")]
    public class when_not_at_design_time : describe_RunnerFactory
    {
        public when_not_at_design_time()
        {
            opts = new CommandLineOptions()
            {
                DotNet = new CommandLineOptions.DotNetPart()
                {
                    DesignTime = false,
                },
            };
        }

        [Test]
        public void it_should_return_console_runner()
        {
            actual.Should().BeOfType(typeof(ConsoleRunner));
        }
    }

    [TestFixture]
    [Category("RunnerFactory")]
    public class when_at_design_time : describe_RunnerFactory
    {
        public when_at_design_time()
        {
            opts = new CommandLineOptions()
            {
                DotNet = new CommandLineOptions.DotNetPart()
                {
                    DesignTime = true,
                },
            };
        }

        [Test]
        public void it_should_return_design_time_runner()
        {
            actual.Should().BeOfType(typeof(DesignTimeRunner));
        }
    }
}
