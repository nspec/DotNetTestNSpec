using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Domain.ConsoleTime;
using DotNetTestNSpec.Domain.DesignTime;
using DotNetTestNSpec.Domain.Library;
using DotNetTestNSpec.Domain.VisualStudio;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;

namespace DotNetTestNSpec.Tests.Domain
{
    [TestFixture]
    [Category("RunnerFactory")]
    public abstract class describe_RunnerFactory
    {
        protected RunnerFactory factory;

        protected ITestRunner actual;
        protected LaunchOptions options;

        protected const string testAssemblyPath = @"some\path\to\assembly";

        [SetUp]
        public virtual void setup()
        {
            var proxyFactory = new Mock<IProxyFactory>();
            var controller = new Mock<IControllerProxy>();

            var channelFactory = new Mock<IChannelFactory>();

            proxyFactory.Setup(f => f.Create(testAssemblyPath)).Returns(controller.Object);

            factory = new RunnerFactory(proxyFactory.Object, channelFactory.Object);
        }
    }

    public abstract class when_project_set : describe_RunnerFactory
    {
        public when_project_set()
        {
            options = new LaunchOptions()
            {
                DotNet = new LaunchOptions.DotNetPart()
                {
                    Project = testAssemblyPath,
                },
            };
        }

        public override void setup()
        {
            base.setup();

            actual = factory.Create(options);
        }
    }

    public class when_port_not_set : when_project_set
    {
        public when_port_not_set()
        {
            options.DotNet.Port = null;
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
            options.DotNet.Port = 123;

            options.NSpec = new LaunchOptions.NSpecPart();
        }
    }

    public class when_list_set : when_port_set
    {
        public when_list_set()
        {
            options.DotNet.List = true;
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
            options.DotNet.WaitCommand = true;
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
            options = new LaunchOptions()
            {
                DotNet = new LaunchOptions.DotNetPart()
                {
                    Project = testAssemblyPath,
                    DesignTime = true,
                    Port = 123,
                },
                NSpec = new LaunchOptions.NSpecPart(),
            };
        }

        [Test]
        public void it_should_throw()
        {
            Action act = () => factory.Create(options);

            act.ShouldThrow<DotNetTestNSpecException>();
        }
    }

    public class when_project_null : describe_RunnerFactory
    {
        public when_project_null()
        {
            options = new LaunchOptions()
            {
                DotNet = new LaunchOptions.DotNetPart()
                {
                    Project = null,
                },
                NSpec = new LaunchOptions.NSpecPart(),
            };
        }

        [Test]
        public void it_should_throw()
        {
            Action act = () => factory.Create(options);

            act.ShouldThrow<DotNetTestNSpecException>();
        }
    }
}
