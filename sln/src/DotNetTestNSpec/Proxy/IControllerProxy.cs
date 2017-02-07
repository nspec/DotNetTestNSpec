using System.Collections.Generic;

namespace DotNetTestNSpec.Proxy
{
    public interface IControllerProxy
    {
        int Run(
            string testAssemblyPath,
            string tags,
            string formatterClassName,
            IDictionary<string, string> formatterOptions,
            bool failFast);
    }
}
