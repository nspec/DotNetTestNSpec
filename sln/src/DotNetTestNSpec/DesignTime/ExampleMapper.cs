using DotNetTestNSpec.Proxy;
using Microsoft.Extensions.Testing.Abstractions;
using System;
using System.Text.RegularExpressions;

namespace DotNetTestNSpec.DesignTime
{
    public class ExampleMapper
    {
        public Test MapToTest(DiscoveredExample example)
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

        public TestResult MapToTestResult(ExecutedExample example, Test test)
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

        string BeautifyForDisplay(string fullName)
        {
            // beautification idea taken from
            // https://github.com/osoftware/NSpecTestAdapter/blob/master/NSpec.TestAdapter/TestCaseDTO.cs

            string displayName;

            // chop leading, redundant 'nspec. ' context, if any

            displayName = prefixRegex.Replace(fullName, prefixReplacement);

            // replace context separator

            displayName = separatorRegex.Replace(displayName, separatorReplacement);

            return displayName;
        }

        readonly Regex prefixRegex = new Regex(@"^nspec\. ");
        readonly Regex separatorRegex = new Regex(@"\. ");

        const string prefixReplacement = "";
        const string separatorReplacement = " › ";
    }
}
