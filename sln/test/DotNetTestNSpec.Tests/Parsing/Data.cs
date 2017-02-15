using System.Collections.Generic;

namespace DotNetTestNSpec.Tests.Parsing
{
    public static class Data
    {
        public static class DotNet
        {
            public const string someProjectPath = @"Path\To\Some\Project";

            public const int someProcessId = 123;
            public const string someProcessIdArg = "123";

            public const int somePort = 456;
            public const string somePortArg = "456";

            public static readonly DotNetCommandLineOptions allOptions = new DotNetCommandLineOptions()
            {
                Project = someProjectPath,
                DesignTime = true,
                List = true,
                WaitCommand = true,
                ParentProcessId = someProcessId,
                Port = somePort,
                NSpecArgs = new string[0],
                UnknownArgs = new string[0],
            };
        }

        public static class NSpec
        {
            public const string someClassName = "someClassName";
            public const string someTags = "tag1,tag2,tag3";
            public const string someFormatterName = "someFormatterName";

            public static readonly string[] someTestNames =
            {
                "test Name 1",
                "test Name 2",
                "test Name 3",
            };
            public const string someTestNamesArg = "test Name 1, test Name 2, test Name 3";

            public static readonly NSpecCommandLineOptions allOptions = new NSpecCommandLineOptions()
            {
                ClassName = someClassName,
                Tags = someTags,
                FailFast = true,
                FormatterName = someFormatterName,
                FormatterOptions = new Dictionary<string, string>()
                {
                    { "optName1", "optValue1" },
                    { "optName2", "optName2" },
                    { "optName3", "optValue3" },
                },
                DebugChannel = true,
                DebugTests = someTestNames,
                UnknownArgs = new string[0],
            };
        }
    }
}
