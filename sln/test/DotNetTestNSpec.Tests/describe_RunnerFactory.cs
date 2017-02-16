using DotNetTestNSpec.ConsoleTime;
using DotNetTestNSpec.DesignTime;
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

        protected const string testAssemblyPath = @"some\path\to\assembly";

        [SetUp]
        public virtual void setup()
        {
            var proxyFactory = new Mock<IProxyFactory>();
            var controller = new Mock<IControllerProxy>();

            proxyFactory.Setup(f => f.Create(testAssemblyPath)).Returns(controller.Object);

            factory = new RunnerFactory(proxyFactory.Object);
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

            actual = factory.Create(opts);
        }
    }

    public class when_port_not_set : when_project_set
    {
        public when_port_not_set()
        {
            opts.DotNet.Port = null;
        }

        [Test]
        public void it_should_return_console_runner()
        {
            actual.Should().BeOfType(typeof(ConsoleRunner));
        }
    }

    public abstract class when_port_set : when_project_set
    {
        public when_port_set()
        {
            opts.DotNet.Port = 123;

            opts.NSpec = new CommandLineOptions.NSpecPart();
        }
    }

    public class when_list_set : when_port_set
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

    public class when_wait_command_set : when_port_set
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

    public class when_no_list_nor_wait_command_set : describe_RunnerFactory
    {
        public when_no_list_nor_wait_command_set()
        {
            opts = new CommandLineOptions()
            {
                DotNet = new CommandLineOptions.DotNetPart()
                {
                    Project = testAssemblyPath,
                    DesignTime = true,
                    Port = 123,
                },
                NSpec = new CommandLineOptions.NSpecPart(),
            };
        }

        [Test]
        public void it_should_throw()
        {
            Action act = () => factory.Create(opts);

            act.ShouldThrow<DotNetTestNSpecException>();
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
                NSpec = new CommandLineOptions.NSpecPart(),
            };
        }

        [Test]
        public void it_should_throw()
        {
            Action act = () => factory.Create(opts);

            act.ShouldThrow<DotNetTestNSpecException>();
        }
    }
}
