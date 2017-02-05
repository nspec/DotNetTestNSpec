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
            var argTail = args.SkipWhile(arg => arg != argKey);
            IEnumerable<string> unusedArgs;

            if (argTail.Any())
            {
                argTail = argTail.Skip(1);  // skip key

                string text = argTail.FirstOrDefault();

                setValue(text);

                argTail = argTail.Skip(1);  // skip value, if any

                unusedArgs = args
                    .TakeWhile(arg => arg != argKey)
                    .Concat(argTail);
            }
            else
            {
                unusedArgs = args;
            }

            return unusedArgs;
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
            IEnumerable<string> args, string argKey, Action setValue)
        {
            var argTail = args.SkipWhile(arg => arg != argKey);
            IEnumerable<string> unusedArgs;

            if (argTail.Any())
            {
                argTail = argTail.Skip(1);  // skip key

                setValue();

                unusedArgs = args
                    .TakeWhile(arg => arg != argKey)
                    .Concat(argTail);
            }
            else
            {
                unusedArgs = args;
            }

            return unusedArgs;
        }
    }
}
