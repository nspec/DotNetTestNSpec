﻿using System.Reflection;

namespace DotNetTestNSpec.Shared
{
    public static class AssemblyUtils
    {
        public static string GetPrintInfo(this Assembly assembly)
        {
            string name = assembly.GetName().Name;

            var versionInfoAttribute = assembly.GetCustomAttribute(typeof(AssemblyInformationalVersionAttribute))
                as AssemblyInformationalVersionAttribute;

            string version = versionInfoAttribute != null
                ? versionInfoAttribute.InformationalVersion
                : assembly.GetName().Version.ToString();

            string versionInfo = $"{name} version: {version}";

            return versionInfo;
        }
    }
}
