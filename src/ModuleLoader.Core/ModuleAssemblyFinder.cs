using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace ModuleLoader.Core
{
    internal static class ModuleAssemblyFinder
    {
        private static IEnumerable<string> GetDllsFromFolder(string folderPath, SearchOption searchOption,
            string moduleBaseName = "")
        {
            return Directory.GetFiles(folderPath, "*.dll", searchOption)
                .Where(s => s.Substring(folderPath.Length + 1).Contains(moduleBaseName));
        }

        public static IList<Assembly> LoadAssembliesFromPath(string folderPath, SearchOption searchOption,
            string moduleBaseName = "")
        {
            return GetDllsFromFolder(folderPath, searchOption, moduleBaseName)
                .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
                .ToList();
        }
    }
}
