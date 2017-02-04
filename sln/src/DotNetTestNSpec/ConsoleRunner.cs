using DotNetTestNSpec.Compatibility;
using DotNetTestNSpec.Parsing;
using System;
using System.IO;
using System.Reflection;

namespace DotNetTestNSpec
{
    public class ConsoleRunner
    {
        public int Run(string[] args)
        {
            var testRunnerAssembly = typeof(Program).GetTypeInfo().Assembly;

            Console.WriteLine(testRunnerAssembly.GetPrintInfo());

            var dotNetArgumentParser = new DotNetArgumentParser();

            DotNetCommandLineOptions dotNetOptions = dotNetArgumentParser.Parse(args);

            if (dotNetOptions.Project == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var nspecArgumentParser = new NSpecArgumentParser();

            NSpecCommandLineOptions nspecOptions = nspecArgumentParser.Parse(dotNetOptions.NSpecArgs);

            var nspecLibraryAssembly = GetNSpecLibraryAssembly(dotNetOptions.Project);

            Console.WriteLine(nspecLibraryAssembly.GetPrintInfo());

            var controllerProxy = new ControllerProxy(nspecLibraryAssembly);

            int nrOfFailures = controllerProxy.Run(
                testAssemblyPath: dotNetOptions.Project,
                tags: nspecOptions.Tags,
                formatterClassName: nspecOptions.FormatterName,
                formatterOptions: nspecOptions.FormatterOptions,
                failFast: nspecOptions.FailFast);

            return nrOfFailures;
        }

        static Assembly GetNSpecLibraryAssembly(string testAssemblyPath)
        {
            string outputAssemblyDirectory = Path.GetDirectoryName(testAssemblyPath);

            string nspecLibraryAssemblyPath = Path.Combine(outputAssemblyDirectory, nspecFileName);

            try
            {
                var nspecLibraryAssembly = AssemblyUtils.LoadFromPath(nspecLibraryAssemblyPath);

                return nspecLibraryAssembly;
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(
                    $"Could not load referenced NSpec library assembly at '{nspecLibraryAssemblyPath}'",
                    ex);
            }
        }

        const string nspecFileName = "NSpec.dll";
    }
}
