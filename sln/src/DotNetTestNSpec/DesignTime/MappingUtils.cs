using DotNetTestNSpec.Proxy;
using Microsoft.Extensions.Testing.Abstractions;
using System;

namespace DotNetTestNSpec.DesignTime
{
    public static class MappingUtils
    {
        public static Test MapToTest(DiscoveredExample example)
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

        public static TestResult MapToTestResult(ExecutedExample example, Test test)
        {
            var testResult = new TestResult(test)
            {
                DisplayName = BeautifyForDisplay(example.FullName),
                ErrorMessage = example.ExceptionMessage,
                ErrorStackTrace = example.ExceptionStackTrace,
                Duration = example.Duration,
                ComputerName = Environment.MachineName,
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
