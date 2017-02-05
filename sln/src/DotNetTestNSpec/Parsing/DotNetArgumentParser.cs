using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public class DotNetArgumentParser
    {
        public DotNetArgumentParser()
        {
            knownArgKeys = new[]
            {
                parentProcessArgKey,
                portArgKey,
            };
        }

        public DotNetCommandLineOptions Parse(string[] args)
        {
            IEnumerable<string> dotNetTestArgs = args.TakeWhile(arg => arg != "--");
            IEnumerable<string> nspecArgs = args.Skip(dotNetTestArgs.Count() + 1);

            var options = new DotNetCommandLineOptions();

            // look for first argument (the project), before remaining dotnet-test options

            string firstArg = dotNetTestArgs.FirstOrDefault();

            if (!knownArgKeys.Contains(firstArg))
            {
                options.Project = firstArg;

                dotNetTestArgs = dotNetTestArgs.Skip(1);
            }

            // look for remaining dotnet-test options

            IEnumerable<string> remainingArgs;

            remainingArgs = ParsingUtils.SetIntForOptionalArg(dotNetTestArgs,
                parentProcessArgKey, value => options.ParentProcessId = value);

            remainingArgs = ParsingUtils.SetIntForOptionalArg(remainingArgs,
                portArgKey, value => options.Port = value);

            remainingArgs = ParsingUtils.SetBoolForSwitchArg(remainingArgs,
                designTimeArgKey, () => options.DesignTime = true);

            remainingArgs = ParsingUtils.SetBoolForSwitchArg(remainingArgs,
                listArgKey, () => options.List = true);

            options.NSpecArgs = nspecArgs.ToArray();

            options.UnknownArgs = remainingArgs.ToArray();

            return options;
        }

        string[] knownArgKeys;

        const string designTimeArgKey = "--designtime";
        const string listArgKey = "--list";
        const string parentProcessArgKey = "--parentProcessId";
        const string portArgKey = "--port";
    }
}
