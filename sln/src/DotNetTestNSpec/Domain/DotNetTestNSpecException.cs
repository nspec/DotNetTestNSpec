using System;

namespace DotNetTestNSpec.Domain
{
    public class DotNetTestNSpecException : Exception
    {
        public DotNetTestNSpecException(string message)
            : base(message)
        { }

        public DotNetTestNSpecException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
