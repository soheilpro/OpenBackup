using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenBackup.Framework;

namespace OpenBackup.Runtime
{
    public class DirectoryExtensionProvider : IExtensionProvider
    {
        public string Path
        {
            get;
            private set;
        }

        public DirectoryExtensionProvider(string path)
        {
            Path = path;
        }

        public IEnumerable<string> GetExtensions()
        {
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += AppDomain_ReflectionOnlyAssemblyResolve;

            if (!Directory.Exists(Path))
                yield break;

            // TODO
            var extensionPaths = from assemblyPath in Directory.EnumerateFiles(Path, "*.dll")
                                 where System.IO.Path.GetFileNameWithoutExtension(assemblyPath).IndexOf("Extension", StringComparison.OrdinalIgnoreCase) != -1
                                 where IsExtensionAssembly(assemblyPath)
                                 select assemblyPath;

            foreach (var extensionPath in extensionPaths)
                yield return extensionPath;

            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve -= AppDomain_ReflectionOnlyAssemblyResolve;
        }

        private static bool IsExtensionAssembly(string assemblyPath)
        {
            var assembly = Assembly.ReflectionOnlyLoadFrom(assemblyPath);

            foreach (var item in assembly.GetCustomAttributesData())
            {
                if (item.Constructor.DeclaringType.Namespace == typeof(ExtensionAttribute).Namespace &&
                    item.Constructor.DeclaringType.Name == typeof(ExtensionAttribute).Name)
                    return true;
            }

            return false;
        }

        private static Assembly AppDomain_ReflectionOnlyAssemblyResolve(object sender, ResolveEventArgs args)
        {
            return Assembly.ReflectionOnlyLoad(args.Name);
        }
    }
}
