using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Parsing;
using DotNetTestNSpec.Proxy;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace DotNetTestNSpec.Tests
{
    [TestFixture]
    [Category("RunnerFactory")]
    public abstract class describe_RunnerFactory
    {
        protected RunnerFactory factory;

        protected ITestRunner actual;
        protected CommandLineOptions opts;

        protected readonly string[] args = { "one", "two", "three" };

        protected const string testAssemblyPath = @"some\path\to\assembly";

        [SetUp]
        public virtual void setup()
        {
            var argumentParser = new Mock<IArgumentParser>();

            argumentParser.Setup(p => p.Parse(args)).Returns(opts);

            var proxyFactory = new Mock<IProxyFactory>();
            var controller = new Mock<IControllerProxy>();

            proxyFactory.Setup(f => f.Create(testAssemblyPath)).Returns(controller.Object);

            factory = new RunnerFactory(argumentParser.Object, proxyFactory.Object);
        }
    }

    public abstract class when_project_set : describe_RunnerFactory
    {
        public when_project_set()
        {
            opts = new CommandLineOptions()
            {
                DotNet = new CommandLineOptions.DotNetPart()
                {
                    Project = testAssemblyPath,
                },
            };
        }

        public override void setup()
        {
            base.setup();

            actual = factory.Create(args);
        }
    }

    public class when_not_at_design_time : when_project_set
    {
        public when_not_at_design_time()
        {
            opts.DotNet.DesignTime = false;
        }

        [Test]
        public void it_should_return_console_runner()
        {
            actual.Should().BeOfType(typeof(ConsoleRunner));
        }
    }

    public abstract class when_at_design_time : when_project_set
    {
        public when_at_design_time()
        {
            opts.DotNet.DesignTime = true;
            opts.DotNet.Port = 123;

            opts.NSpec = new CommandLineOptions.NSpecPart();
        }
    }

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

    public class when_project_null : describe_RunnerFactory
    {
        public when_project_null()
        {
            opts = new CommandLineOptions()
            {
                DotNet = new CommandLineOptions.DotNetPart()
                {
                    Project = null,
                },
            };
        }

        [Test]
        public void it_should_throw()
        {
            Action act = () => factory.Create(args);

            act.ShouldThrow<DotNetTestNSpecException>();
        }
    }
}
