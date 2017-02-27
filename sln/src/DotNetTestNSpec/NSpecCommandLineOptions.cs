﻿using System.Collections.Generic;

namespace DotNetTestNSpec
{
    public class NSpecCommandLineOptions : CommandLineOptions.NSpecPart
    {
        public string[] UnknownArgs { get; set; }

        public override string ToString()
        {
            return EnumerableUtils.ToObjectString(new string[]
            {
                $"{nameof(ClassName)}: {ClassName}",
                $"{nameof(Tags)}: {Tags}",
                $"{nameof(FailFast)}: {FailFast}",
                $"{nameof(FormatterName)}: {FormatterName}",
                $"{nameof(FormatterOptions)}: {DictionaryUtils.ToArrayString(FormatterOptions)}",
                $"{nameof(UnknownArgs)}: {EnumerableUtils.ToArrayString(UnknownArgs)}",
            }, true);
        }
    }
}
