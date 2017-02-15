using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public static class ParsingUtils
    {
        public static IEnumerable<string> SetTextForOptionalArg(
            IEnumerable<string> args, string argKey, IEnumerable<string> knownTokens, Action<string> setValue)
        {
            return SetValueForOptionalArg(args, argKey, text =>
            {
                bool argValueMissing = (text == null) || knownTokens.Any(key => text.StartsWith(key, StringComparison.Ordinal));

                if (argValueMissing)
                {
                    throw new ArgumentException($"Argument '{argKey}' must be followed by its value, instead found: '{text}'.");
                }

                setValue(text);
            });
        }

        public static IEnumerable<string> SetIntForOptionalArg(
            IEnumerable<string> args, string argKey, Action<int> setValue)
        {
            return SetValueForOptionalArg(args, argKey, text =>
            {
                int value;
                bool argValueFound = Int32.TryParse(text, out value);

                if (!argValueFound)
                {
                    throw new ArgumentException($"Argument '{argKey}' must be followed by its value, instead found: '{text}'.");
                }

                setValue(value);
            });
        }

        public static IEnumerable<string> SetTextForOptionalArgPrefix(
            IEnumerable<string> args, string argPrefix, Action<string> setValue)
        {
            string foundArg = args
                .FirstOrDefault(arg => arg.StartsWith(argPrefix, StringComparison.Ordinal));

            if (foundArg == null)
            {
                return args;
            }

            string[] tokens = foundArg.Split(new[] { argPrefix }, StringSplitOptions.RemoveEmptyEntries);

            string value = tokens.First();

            setValue(value);

            return args.Where(arg => arg != foundArg);
        }

        public static IEnumerable<string> SetBoolForSwitchArg(
            IEnumerable<string> args, string argKey, Action<bool> setValue)
        {
            bool hasKey = args.Contains(argKey);

            setValue(hasKey);

            var unusedArgs = hasKey
                ? args.Where(arg => arg != argKey)
                : args;

            return unusedArgs;
        }

        static IEnumerable<string> SetValueForOptionalArg(
            IEnumerable<string> args, string argKey, Action<string> setValue)
        {
            return ProcessArgument(args, argKey, argTail =>
            {
                string textValue = argTail.FirstOrDefault();

                argTail = argTail.Skip(1);  // skip value, if any

                setValue(textValue);

                return argTail;
            });
        }

        static IEnumerable<string> ProcessArgument(
            IEnumerable<string> args, string argKey, Func<IEnumerable<string>, IEnumerable<string>> processTail)
        {
            IEnumerable<string> unusedArgs;

            var argTail = args.SkipWhile(arg => arg != argKey);

            if (argTail.Any())
            {
                argTail = argTail.Skip(1);  // skip key

                var headArgs = args
                    .TakeWhile(arg => arg != argKey);

                var unprocessedArgs = processTail(argTail);

                unusedArgs = headArgs
                    .Concat(unprocessedArgs);
            }
            else
            {
                unusedArgs = args;
            }

            return unusedArgs;
        }
    }
}
