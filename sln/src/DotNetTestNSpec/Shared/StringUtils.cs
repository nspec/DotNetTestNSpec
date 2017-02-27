using System.Diagnostics;

namespace DotNetTestNSpec.Shared
{
    public static class StringUtils
    {
        /// <summary>
        /// Extension method that wraps String.Format.
        /// <para>Usage: string result = "{0} {1}".With("hello", "world");</para>
        /// </summary>
        [DebuggerNonUserCode]
        public static string With(this string source, params object[] objects)
        {
            return string.Format(source, objects);
        }
    }
}
