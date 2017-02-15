using DotNetTestNSpec.Compatibility;
using DotNetTestNSpec.Parsing;
using DotNetTestNSpec.Proxy;
using System;
using System.Reflection;

namespace DotNetTestNSpec
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var testRunnerAssembly = typeof(Program).GetTypeInfo().Assembly;

            Console.WriteLine(testRunnerAssembly.GetPrintInfo());

#if true
            // TODO remove when feature is implemented

            Console.WriteLine("-- BEGIN --");
            Console.WriteLine("Input arguments:");

            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }

            Console.WriteLine("--- END ---");
#endif

            var argumentParser = new ArgumentParser();

            var runnerFactory = new RunnerFactory(new ProxyFactory());

            try
            {
                var commandLineOptions = argumentParser.Parse(args);

                var runner = runnerFactory.Create(commandLineOptions);

                runner.Start();

                return ReturnCodes.Ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return ReturnCodes.Error;
            }
        }

        public static class ReturnCodes
        {
            public const int Ok = 0;
            public const int Error = -1;
        }
    }
}
