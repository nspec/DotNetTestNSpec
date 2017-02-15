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
                designTimeArgKey,
                listArgKey,
                waitCommandArgKey,
                parentProcessArgKey,
                portArgKey,
            };
        }

        public DotNetCommandLineOptions Parse(IEnumerable<string> args)
        {
            IEnumerable<string> dotNetTestArgs = args.TakeWhile(arg => arg != nspecArgDivider);
            IEnumerable<string> nspecArgs = args.Skip(dotNetTestArgs.Count() + 1);

            var options = new DotNetCommandLineOptions();

            // look for first argument (the project), before remaining dotnet-test options

            string firstArg = dotNetTestArgs.FirstOrDefault();

            if (IsUnknownArgument(firstArg))
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
                designTimeArgKey, value => options.DesignTime = value);

            remainingArgs = ParsingUtils.SetBoolForSwitchArg(remainingArgs,
                listArgKey, value => options.List = value);

            remainingArgs = ParsingUtils.SetBoolForSwitchArg(remainingArgs,
                waitCommandArgKey, value => options.WaitCommand = value);

            options.NSpecArgs = nspecArgs.ToArray();

            options.UnknownArgs = remainingArgs.ToArray();

            return options;
        }

        bool IsUnknownArgument(string arg)
        {
            return !knownArgKeys.Contains(arg);
        }

        readonly string[] knownArgKeys;

        const string designTimeArgKey = "--designtime";
        const string listArgKey = "--list";
        const string waitCommandArgKey = "--wait-command";
        const string parentProcessArgKey = "--parentProcessId";
        const string portArgKey = "--port";
        const string nspecArgDivider = "--";
    }
}
