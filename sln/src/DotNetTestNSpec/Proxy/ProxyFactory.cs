using DotNetTestNSpec.Compatibility;
using System;
using System.IO;
using System.Reflection;

namespace DotNetTestNSpec.Proxy
{
    public class ProxyFactory
    {
        public ControllerProxy Create(string testAssemblyPath)
        {
            var nspecLibraryAssembly = GetNSpecLibraryAssembly(testAssemblyPath);

            Console.WriteLine(nspecLibraryAssembly.GetPrintInfo());

            var controllerProxy = new ControllerProxy(nspecLibraryAssembly);

            return controllerProxy;
        }

        const string nspecFileName = "NSpec.dll";

        static Assembly GetNSpecLibraryAssembly(string testAssemblyPath)
        {
            string outputAssemblyDirectory = Path.GetDirectoryName(testAssemblyPath);

            string nspecLibraryAssemblyPath = Path.Combine(outputAssemblyDirectory, nspecFileName);

            try
            {
                var nspecLibraryAssembly = AssemblyUtils.LoadFromPath(nspecLibraryAssemblyPath);

                return nspecLibraryAssembly;
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(
                    $"Could not load referenced NSpec library assembly at '{nspecLibraryAssemblyPath}'",
                    ex);
            }
        }
    }
}
