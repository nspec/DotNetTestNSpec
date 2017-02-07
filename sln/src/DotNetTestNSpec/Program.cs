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

            Console.WriteLine("Input arguments: begin");

            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }

            Console.WriteLine("Input arguments: end.");
#endif

            var runnerFactory = new RunnerFactory(new ArgumentParser(), new ProxyFactory());

            var runner = runnerFactory.Create(args);

            try
            {
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
