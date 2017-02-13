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
    [Category("ExecutionRunner")]
    public abstract class describe_ExecutionRunner
    {
        protected ExecutionRunner runner;

        protected Mock<IControllerProxy> controllerProxy;
        protected Mock<IExecutionAdapter> adapter;

        protected const string testAssemblyPath = @"some\path\to\assembly";
        protected const string codeAssemblyPath = @"another\path\to\assembly";

        [SetUp]
        public virtual void setup()
        {
            controllerProxy = new Mock<IControllerProxy>();
            adapter = new Mock<IExecutionAdapter>();

            runner = new ExecutionRunner(testAssemblyPath, adapter.Object, controllerProxy.Object);
        }

        protected string BuildFullName(int i)
        {
            return $"nspec. A class. A context. Some Example-{i}";
        }
    }

    public class when_execution_started : describe_ExecutionRunner
    {
        List<Test> actualStartedTests;
        List<TestResult> actualCompletedResults;

        readonly IEnumerable<Test> expectedStartedTests;
        readonly List<TestResult> expectedResults;

        readonly IEnumerable<string> requestedTestFullNames;
        readonly IEnumerable<DiscoveredExample> discoveredExamples;
        readonly List<ExecutedExample> executedExamples;

        const string someErrorMessage = "Some error message";
        const string someErrorStackTrace = "Some stack trace";

        public when_execution_started()
        {
            var indexes = Enumerable.Range(1, 3);

            requestedTestFullNames =
                from i in indexes
                select BuildFullName(i);

            discoveredExamples =
                from i in indexes
                select new DiscoveredExample()
                {
                    FullName = BuildFullName(i),
                    SourceFilePath = $@"some\path\to\code-{i}",
                    SourceLineNumber = 10 * i,
                    SourceAssembly = codeAssemblyPath,
                    Tags = new[] { $"tag-{i}A", $"tag-{i}B", $"tag-{i}C" },
                };

            executedExamples = (
                from i in indexes
                select new ExecutedExample()
                {
                    FullName = BuildFullName(i),
                })
                .ToList();

            executedExamples[1].Pending = true;

            executedExamples[2].Failed = true;
            executedExamples[2].ExceptionMessage = someErrorMessage;
            executedExamples[2].ExceptionStackTrace = someErrorStackTrace;

            expectedStartedTests =
                from i in indexes
                select new Test()
                {
                    FullyQualifiedName = BuildFullName(i),
                    DisplayName = $"A class › A context › Some Example-{i}",
                    CodeFilePath = $@"some\path\to\code-{i}",
                    LineNumber = 10 * i,
                };

            expectedResults = (
                from test in expectedStartedTests
                select new TestResult(test)
                {
                    DisplayName = test.DisplayName,
                })
                .ToList();

            expectedResults[0].Outcome = TestOutcome.Passed;

            expectedResults[1].Outcome = TestOutcome.Skipped;

            expectedResults[2].Outcome = TestOutcome.Failed;
            expectedResults[2].ErrorMessage = someErrorMessage;
            expectedResults[2].ErrorStackTrace = someErrorStackTrace;
        }

        public override void setup()
        {
            base.setup();

            controllerProxy
                .Setup(c => c.Execute(testAssemblyPath, requestedTestFullNames, It.IsAny<IExecutionSink>()))
                .Callback((string _, IEnumerable<string> __, IExecutionSink runSink) =>
                {
                    foreach (var fullName in requestedTestFullNames)
                    {
                        var discovered = discoveredExamples
                            .Where(exm => exm.FullName == fullName)
                            .Single();

                        runSink.ExampleStarted(discovered);

                        var executed = executedExamples
                            .Where(exm => exm.FullName == fullName)
                            .Single();

                        runSink.ExampleCompleted(executed);
                    }
                });

            adapter.Setup(a => a.Connect()).Returns(requestedTestFullNames);

            adapter.Setup(a => a.TestStarted(It.IsAny<Test>())).Callback((Test test) =>
            {
                actualStartedTests.Add(test);
            });

            actualStartedTests = new List<Test>();

            adapter.Setup(a => a.TestFinished(It.IsAny<TestResult>())).Callback((TestResult result) =>
            {
                actualCompletedResults.Add(result);
            });

            actualCompletedResults = new List<TestResult>();

            runner.Start();
        }

        [Test]
        public void it_should_connect_adapter()
        {
            adapter.Verify(a => a.Connect(), Times.Once);
        }

        [Test]
        public void it_should_notify_each_started_test()
        {
            actualStartedTests.ShouldBeEquivalentTo(expectedStartedTests);
        }

        [Test]
        public void it_should_notify_each_completed_test_result()
        {
            actualCompletedResults.ShouldBeEquivalentTo(expectedResults);
        }

        [Test]
        public void it_should_disconnect_adapter()
        {
            adapter.Verify(a => a.Disconnect(), Times.Once);
        }
    }
}
