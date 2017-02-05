using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetTestNSpec.Parsing
{
    public static class ParsingUtils
    {
        public static IEnumerable<string> SetTextForOptionalArg(
            IEnumerable<string> args, string argKey, Action<string> setValue)
        {
            return ProcessArgument(args, argKey, argTail =>
            {
                string text = argTail.FirstOrDefault();

                setValue(text);

                argTail = argTail.Skip(1);  // skip value, if any

                return argTail;
            });
        }

        public static IEnumerable<string> SetIntForOptionalArg(
            IEnumerable<string> args, string argKey, Action<int> setValue)
        {
            return SetTextForOptionalArg(args, argKey, text =>
            {
                int value;
                bool argValueFound = Int32.TryParse(text, out value);

                if (!argValueFound)
                {
                    throw new ArgumentException($"Argument '{argKey}' must be followed by its value");
                }

                setValue(value);
            });
        }

        public static IEnumerable<string> SetBoolForSwitchArg(
            IEnumerable<string> args, string argKey, Action setTrue)
        {
            return ProcessArgument(args, argKey, argTail =>
            {
                setTrue();

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
