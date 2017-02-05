using DotNetTestNSpec.Compatibility;
using System;
using System.IO;
using System.Reflection;

namespace DotNetTestNSpec
{
    public class ConsoleRunner : ITestRunner
    {
        public ConsoleRunner(CommandLineOptions commandLineOptions)
        {
            this.commandLineOptions = commandLineOptions;
        }

        public int Start()
        {
            var testRunnerAssembly = typeof(Program).GetTypeInfo().Assembly;

            Console.WriteLine(testRunnerAssembly.GetPrintInfo());

            EnsureOptionsValid();

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

        void EnsureOptionsValid()
        {
            if (commandLineOptions.DotNet.Project == null)
            {
                throw new DotNetTestNSpecException("Command line arguments must include path of test project assembly");
            }
        }

        readonly CommandLineOptions commandLineOptions;

        const string nspecFileName = "NSpec.dll";

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
    }
}
