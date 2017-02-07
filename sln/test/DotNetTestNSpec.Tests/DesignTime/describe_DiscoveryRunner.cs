using DotNetTestNSpec.DesignTime;
using DotNetTestNSpec.Proxy;
using FluentAssertions;
using Microsoft.Extensions.Testing.Abstractions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Tests.DesignTime
{
    [TestFixture]
    [Category("DiscoveryRunner")]
    public abstract class describe_DiscoveryRunner
    {
        protected DiscoveryRunner runner;

        protected Mock<IControllerProxy> controllerProxy;
        protected Mock<IDiscoveryAdapter> adapter;

        protected readonly CommandLineOptions opts = new CommandLineOptions()
        {
            DotNet = new CommandLineOptions.DotNetPart()
            {
                Project = testAssemblyPath,
            },
        };
        protected const string testAssemblyPath = @"some\path\to\assembly";
        protected const string codeAssemblyPath = @"another\path\to\assembly";

        [SetUp]
        public virtual void setup()
        {
            controllerProxy = new Mock<IControllerProxy>();
            adapter = new Mock<IDiscoveryAdapter>();

            runner = new DiscoveryRunner(opts, controllerProxy.Object, adapter.Object);
        }
    }

    public class when_started : describe_DiscoveryRunner
    {
        List<DiscoveredExample> foundTests;

        readonly IEnumerable<DiscoveredExample> discoveredExamples;

        public when_started()
        {
            var indexes = Enumerable.Range(1, 3);

            discoveredExamples =
                from i in indexes
                select new DiscoveredExample()
                {
                    FullName = $"Some-Example-{i}",
                    SourceFilePath = $@"some\path\to\code-{i}",
                    SourceLineNumber = 10 * i,
                    SourceAssembly = codeAssemblyPath,
                    Tags = new[] { $"tag-{i}A", $"tag-{i}B", $"tag-{i}C" },
                };
        }

        public override void setup()
        {
            base.setup();

            controllerProxy.Setup(c => c.List(testAssemblyPath)).Returns(discoveredExamples);

            adapter.Setup(a => a.TestFound(It.IsAny<DiscoveredExample>())).Callback((DiscoveredExample example) =>
            {
                foundTests.Add(example);
            });

            foundTests = new List<DiscoveredExample>();

            runner.Start();
        }

        [Test]
        public void it_should_connect_adapter()
        {
            adapter.Verify(a => a.Connect(), Times.Once);
        }

        [Test]
        public void it_should_notify_each_discovered_test()
        {
            foundTests.ShouldBeEquivalentTo(discoveredExamples);
        }

        [Test]
        public void it_should_disconnect_adapter()
        {
            adapter.Verify(a => a.Disconnect(), Times.Once);
        }
    }
}
