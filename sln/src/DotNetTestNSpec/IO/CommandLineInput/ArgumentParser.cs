using DotNetTestNSpec.Domain;
using System.Linq;

namespace DotNetTestNSpec.IO.CommandLineInput
{
    public class ArgumentParser : IArgumentParser
    {
        public LaunchOptions Parse(string[] args)
        {
            var dotNetArgumentParser = new DotNetArgumentParser();

            DotNetCommandLineOptions dotNetOptions = dotNetArgumentParser.Parse(args);

            var nspecArgumentParser = new NSpecArgumentParser();

            NSpecCommandLineOptions nspecOptions = nspecArgumentParser.Parse(dotNetOptions.NSpecArgs);

            var allUnknownArgs = dotNetOptions.UnknownArgs
                .Concat(nspecOptions.UnknownArgs);

            var options = new LaunchOptions(dotNetOptions, nspecOptions, allUnknownArgs);

            return options;
        }
    }
}
