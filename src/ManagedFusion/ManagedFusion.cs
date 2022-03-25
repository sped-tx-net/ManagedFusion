using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace System.Management.Fusion
{
    public static class ManagedFusion
    {
        public static AssemblyCache GetGlobalAssemblyCache() =>
            AssemblyCache.GetGlobalAssemblyCache();

        public static IEnumerable<Assembly> GetGlobalAssemblies() =>
            AssemblyCache.GetAssemblies();

        public static void InstallAssembly(string assemblyFile)
        {
            var assemblyName = Path.GetFileNameWithoutExtension(assemblyFile);
            var cache = new AssemblyCache(InstallerDescription.CreateForFile(assemblyName, assemblyFile));
            var name = new AssemblyName(assemblyName);
            name.CodeBase = assemblyFile;
            cache.InstallAssembly(name, InstallBehavior.Default);
        }

        public static void UninstallAssembly(string assemblyName)
        {
            var name = new AssemblyName(assemblyName);
            var cache = new AssemblyCache(InstallerDescription.CreateForFile(assemblyName, assemblyName));
            cache.UninstallAssembly(name);
        }

    }
}
