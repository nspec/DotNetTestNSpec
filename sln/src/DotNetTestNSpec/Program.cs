using System;

namespace DotNetTestNSpec
{
    public class Program
    {
        public static int Main(string[] args)
        {
#if true
            // TODO remove when feature is implemented

            Console.WriteLine("Input arguments: begin");

            foreach (string arg in args)
            {
                Console.WriteLine(arg);
            }

            Console.WriteLine("Input arguments: end.");
#endif

            var consoleRunner = new ConsoleRunner();

            try
            {
                consoleRunner.Run(args);

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
