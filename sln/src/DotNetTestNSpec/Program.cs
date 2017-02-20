using DotNetTestNSpec.Domain;
using DotNetTestNSpec.IO.CommandLineInput;
using DotNetTestNSpec.IO.Library;
using DotNetTestNSpec.IO.VisualStudio;
using DotNetTestNSpec.Shared;
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

            var argumentParser = new ArgumentParser();

            try
            {
                var options = argumentParser.Parse(args);

                var channelFactory = new ChannelFactory(options);

                var runnerFactory = new RunnerFactory(new ProxyFactory(), channelFactory);

                var runner = runnerFactory.Create(options);

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
