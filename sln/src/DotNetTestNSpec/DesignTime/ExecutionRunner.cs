using DotNetTestNSpec.Proxy;
using Microsoft.Extensions.Testing.Abstractions;
using System.Collections.Generic;

namespace DotNetTestNSpec.DesignTime
{
    public class ExecutionRunner : ITestRunner, IExecutionSink
    {
        public ExecutionRunner(string testAssemblyPath, IControllerProxy controllerProxy,
            IExecutionAdapter adapter)
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

            controllerProxy.Execute(testAssemblyPath, requestedTestFullNames, this);

            adapter.Disconnect();

            return dontCare;
        }

        // IRunSink

        public void ExampleStarted(DiscoveredExample example)
        {
            var test = MappingUtils.MapToTest(example);

            startedTestMap[example.FullName] = test;

            adapter.TestStarted(test);
        }

        public void ExampleCompleted(ExecutedExample example)
        {
            var test = startedTestMap[example.FullName];

            var testResult = MappingUtils.MapToTestResult(example, test);

            adapter.TestFinished(testResult);
        }

        IDictionary<string, Test> startedTestMap;

        readonly string testAssemblyPath;
        readonly IControllerProxy controllerProxy;
        readonly IExecutionAdapter adapter;

        const int dontCare = -1;
    }
}
