using DotNetTestNSpec.Domain;
using DotNetTestNSpec.Domain.Library;
using DotNetTestNSpec.Shared;
using System;
using System.IO;
using System.Reflection;

namespace DotNetTestNSpec.IO.Library
{
    public class ProxyFactory : IProxyFactory
    {
        public IControllerProxy Create(string testAssemblyPath)
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
                var nspecLibraryAssembly = LoadFromPath(nspecLibraryAssemblyPath);

                return nspecLibraryAssembly;
            }
            catch (Exception ex)
            {
                throw new DotNetTestNSpecException(
                    $"Could not load referenced NSpec library assembly at '{nspecLibraryAssemblyPath}'",
                    ex);
            }
        }

        static Assembly LoadFromPath(string filePath)
        {
            Assembly assembly;

#if NET451
            assembly = Assembly.LoadFrom(filePath);
#else
            var assemblyName = Path.GetFileNameWithoutExtension(filePath);

            assembly = Assembly.Load(new AssemblyName(assemblyName));
#endif

            return assembly;
        }
    }
}
