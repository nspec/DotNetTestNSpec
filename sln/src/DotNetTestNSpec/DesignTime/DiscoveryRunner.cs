using DotNetTestNSpec.Proxy;
using Microsoft.Extensions.Testing.Abstractions;
using System;

namespace DotNetTestNSpec.DesignTime
{
    public class DiscoveryRunner : ITestRunner
    {
        public DiscoveryRunner(string testAssemblyPath, IDiscoveryAdapter adapter,
            IControllerProxy controllerProxy)
        {
            this.testAssemblyPath = testAssemblyPath;
            this.controllerProxy = controllerProxy;
            this.adapter = adapter;
        }

        public int Start()
        {
            adapter.Connect();

            var discoveredExamples = controllerProxy.List(testAssemblyPath);

            foreach (var example in discoveredExamples)
            {
                var test = MapToTest(example);

                adapter.TestFound(test);
            }

            adapter.Disconnect();

            return dontCare;
        }

        readonly string testAssemblyPath;
        readonly IControllerProxy controllerProxy;
        readonly IDiscoveryAdapter adapter;

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
