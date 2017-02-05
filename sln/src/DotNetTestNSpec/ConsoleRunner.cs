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

            var argumentParser = new ArgumentParser();

            CommandLineOptions commandLineOptions = argumentParser.Parse(args);

            if (commandLineOptions.DotNet.Project == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }

            var nspecLibraryAssembly = GetNSpecLibraryAssembly(commandLineOptions.DotNet.Project);

            Console.WriteLine(nspecLibraryAssembly.GetPrintInfo());

            var controllerProxy = new ControllerProxy(nspecLibraryAssembly);

            int nrOfFailures = controllerProxy.Run(
                testAssemblyPath: commandLineOptions.DotNet.Project,
                tags: commandLineOptions.NSpec.Tags,
                formatterClassName: commandLineOptions.NSpec.FormatterName,
                formatterOptions: commandLineOptions.NSpec.FormatterOptions,
                failFast: commandLineOptions.NSpec.FailFast);

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
