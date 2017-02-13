using DotNetTestNSpec.Proxy;
using Microsoft.Extensions.Testing.Abstractions;
using System;
using System.Collections.Generic;

namespace DotNetTestNSpec.DesignTime
{
    public class ExecutionRunner : ITestRunner, IRunSink
    {
        public ExecutionRunner(string testAssemblyPath, IExecutionAdapter adapter,
            IControllerProxy controllerProxy)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.controllerProxy = controllerProxy;
            this.adapter = adapter;
        }

        public int Start()
        {
            if (startedTestMap != null)
            {
                startedTestMap.Clear();
            }

            startedTestMap = new Dictionary<string, Test>();

            var requestedTestFullNames = adapter.Connect();

            controllerProxy.Run(testAssemblyPath, requestedTestFullNames, this);

            adapter.Disconnect();

            return dontCare;
        }

        // IRunSink

        public void ExampleStarted(DiscoveredExample example)
        {
            var test = MapToTest(example);

            startedTestMap[example.FullName] = test;

            adapter.TestStarted(test);
        }

        public void ExampleCompleted(ExecutedExample example)
        {
            var test = startedTestMap[example.FullName];

            var testResult = MapToTestResult(example, test);

            adapter.TestFinished(testResult);
        }

        IDictionary<string, Test> startedTestMap;

        readonly string testAssemblyPath;
        readonly IControllerProxy controllerProxy;
        readonly IExecutionAdapter adapter;

        const int dontCare = -1;

        static Test MapToTest(DiscoveredExample example)
        {
            var test = new Test()
            {
                FullyQualifiedName = example.FullName,
                DisplayName = BeautifyForDisplay(example.FullName),
                CodeFilePath = example.SourceFilePath,
                LineNumber = example.SourceLineNumber,
            };

            return test;
        }

        static TestResult MapToTestResult(ExecutedExample example, Test test)
        {
            var testResult = new TestResult(test)
            {
                DisplayName = BeautifyForDisplay(example.FullName),
                ErrorMessage = example.ExceptionMessage,
                ErrorStackTrace = example.ExceptionStackTrace,
            };

            if (example.Pending)
            {
                testResult.Outcome = TestOutcome.Skipped;
            }
            else if (example.Failed)
            {
                testResult.Outcome = TestOutcome.Failed;
            }
            else
            {
                testResult.Outcome = TestOutcome.Passed;
            }

            return testResult;
        }

        static string BeautifyForDisplay(string fullName)
        {
            // beautification idea taken from
            // https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/TestCaseDTO.cs

            string displayName;

            // chop leading, redundant 'nspec. ' context, if any

            const string nspecPrefix = @"nspec. ";
            const int prefixLength = 7;

            displayName = fullName.StartsWith(nspecPrefix, StringComparison.OrdinalIgnoreCase)
                ? fullName.Substring(prefixLength)
                : fullName;

            // replace context separator

            const string originalSeparator = @". ";
            const string displaySeparator = @" › ";

            displayName = displayName.Replace(originalSeparator, displaySeparator);

            return displayName;
        }
    }
}
