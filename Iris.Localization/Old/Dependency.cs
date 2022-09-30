using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Iris.Localization.Old
{
    public static class Dependency
    {
        //public static IServiceCollection AddMeLocalization(this IServiceCollection collection)
        //{
        //    AssemblyHelper.SetOptions(x => { x.DomainNames = new string[] { "Iris" }; });
        //    AssemblyHelper.LoadAssemblies();

        //    return collection;
        //}


        private static void LoadAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var rootAssembly = Assembly.GetEntryAssembly();

            var visited = new HashSet<string>();
            var queue = new Queue<Assembly>();

            queue.Enqueue(rootAssembly);

            while (queue.Any())
            {
                var assembly = queue.Dequeue();

                if (assembly.FullName.Contains("Iris"))
                {
                    var aa = "";
                }

                GetAllLocalizationResourceNames(assembly);
                visited.Add(assembly.FullName);

                var references = assembly.GetReferencedAssemblies();
                foreach (var reference in references)
                {
                    if (!visited.Contains(reference.FullName))
                        queue.Enqueue(Assembly.Load(reference));
                }


                // do whatever you want with the current Assembly here...
            }


        }

        private static string[] GetAllLocalizationResourceNames(Assembly assembly)
        {
            var resourceNames = assembly.GetManifestResourceNames();

            if (resourceNames.GetLength(0) == 0)
            {
                return resourceNames;
            }

            var localizationResourceNames = resourceNames.Where(r => r.EndsWithPattern("Resources..{5}.json")).ToArray();

            if (localizationResourceNames.Length > 0)
            {
                var aa = localizationResourceNames;
            }
            return localizationResourceNames;
        }
    }

    public static class StringExtentions
    {
        internal static bool StartsWithPattern(this string str, params string[] patterns)
        {
            return patterns.Any(pattern => Regex.Match(str, "^(" + pattern + ")").Success);
        }

        internal static bool EndsWithPattern(this string str, params string[] patterns)
        {
            return patterns.Any(pattern => Regex.Match(str, "(" + pattern + ")$").Success);
        }

        internal static bool ContainsPattern(this string str, params string[] patterns)
        {
            return patterns.Any(pattern => Regex.Match(str, ".*(" + pattern + ").*").Success);
        }
    }
}
