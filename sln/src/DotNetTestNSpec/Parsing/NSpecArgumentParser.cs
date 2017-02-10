using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public class NSpecArgumentParser
    {
        public NSpecArgumentParser()
        {
            knownArgKeys = new[]
            {
                tagsKey,
                failFastKey,
                debugChannelKey,
            };

            knownArgPrefixes = new[]
            {
                formatterPrefix,
                formatterOptionsPrefix,
            };
        }

        public NSpecCommandLineOptions Parse(string[] args)
        {
            IEnumerable<string> remainingArgs;

            // set default option values

            var options = new NSpecCommandLineOptions()
            {
                ClassName = null,
                Tags = null,
                FailFast = false,
                FormatterName = null,
                FormatterOptions = new Dictionary<string, string>(),
                DebugChannel = false,
                UnknownArgs = new string[0],
            };

            // check for first argument (the class name), before remaining named options

            string firstArg = args.FirstOrDefault();

            if (IsUnknownArgument(firstArg))
            {
                options.ClassName = firstArg;

                remainingArgs = args.Skip(1);
            }
            else
            {
                remainingArgs = args;
            }

            // check for remaining named options

            remainingArgs = ParsingUtils.SetTextForOptionalArg(remainingArgs,
                tagsKey, value => options.Tags = value);

            remainingArgs = ParsingUtils.SetBoolForSwitchArg(remainingArgs,
                failFastKey, value => options.FailFast = value);

            remainingArgs = ParsingUtils.SetTextForOptionalArgPrefix(remainingArgs,
                formatterPrefix, value => options.FormatterName = value);

            remainingArgs = ParsingUtils.SetBoolForSwitchArg(remainingArgs,
                debugChannelKey, value => options.DebugChannel = value);

            int lastCount;

            do
            {
                lastCount = remainingArgs.Count();

                remainingArgs = ParsingUtils.SetTextForOptionalArgPrefix(remainingArgs,
                    formatterOptionsPrefix, text =>
                    {
                        string[] tokens = text.Split('=');

                        if (tokens.Length > 2)
                        {
                            throw new ArgumentException(
                                $"Formatter option '{text}' must be either a single 'flag' or a 'name=value' pair");
                        }

                        string name = tokens.First();
                        string value = tokens.Last();

                        options.FormatterOptions[name] = value;
                    });

            } while (lastCount != remainingArgs.Count());

            options.UnknownArgs = remainingArgs.ToArray();

            return options;
        }

        bool IsUnknownArgument(string arg)
        {
            if (knownArgKeys.Contains(arg)) return false;

            bool isKnownPrefix = knownArgPrefixes
                .Where(pfx => arg.StartsWith(pfx, StringComparison.Ordinal))
                .Any();

            return !isKnownPrefix;
        }

        readonly string[] knownArgKeys;
        readonly string[] knownArgPrefixes;

        const string tagsKey = "--tag";
        const string failFastKey = "--failfast";
        const string formatterPrefix = "--formatter=";
        const string formatterOptionsPrefix = "--formatterOptions:";
        const string debugChannelKey = "--debugChannel";
    }
}
