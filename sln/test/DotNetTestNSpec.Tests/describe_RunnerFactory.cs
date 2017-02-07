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

    public abstract class when_at_design_time : describe_RunnerFactory
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
    }

    [TestFixture]
    [Category("RunnerFactory")]
    public class when_list_set : when_at_design_time
    {
        public when_list_set()
        {
            opts.DotNet.List = true;
        }

        [Test]
        public void it_should_return_discovery_runner()
        {
            actual.Should().BeOfType(typeof(DiscoveryRunner));
        }
    }

    [TestFixture]
    [Category("RunnerFactory")]
    public class when_wait_command_set : when_at_design_time
    {
        public when_wait_command_set()
        {
            opts.DotNet.WaitCommand = true;
        }

        [Test]
        public void it_should_return_execution_runner()
        {
            actual.Should().BeOfType(typeof(ExecutionRunner));
        }
    }
}
