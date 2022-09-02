using Microsoft.Extensions.Localization;
using System.Reflection;

namespace Iris
{

    public class AssemblyOptions
    {
        public string[]? DomainNames { get; set; }
    }

    public static class AssemblyHelper
    {
        public static readonly AssemblyOptions options = new();
        public static Assembly[]? assemblies;
        public static HashSet<Assembly> domainAssemblies = new();
        private static bool _isAssembliesLoaded;

        public static void SetOptions(Action<AssemblyOptions>? optionsAction)
        {
            optionsAction?.Invoke(options);
        }

        public static void LoadAssemblies()
        {
            if (_isAssembliesLoaded)
                return;

            assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var rootAssembly = Assembly.GetEntryAssembly();

            if (rootAssembly != null)
            {

                var visited = new HashSet<string>();
                var queue = new Queue<Assembly>();

                queue.Enqueue(rootAssembly);

                while (queue.Any())
                {
                    var assembly = queue.Dequeue();

                    //If assembly is an domain assembly
                    if (options.DomainNames?.Any(x => assembly.FullName.StartsWith($"{x}.")) ?? false)
                    {
                        domainAssemblies.Add(assembly);
                    }

                    visited.Add(assembly.FullName);

                    var references = assembly.GetReferencedAssemblies();
                    foreach (var reference in references)
                    {
                        if (!visited.Contains(reference.FullName))
                            queue.Enqueue(Assembly.Load(reference));
                    }

                }
            }

            _isAssembliesLoaded = true;
        }


        public static void SearchinDomain()
        {
            foreach(var assemb in domainAssemblies)
            {
                var resourceLocationAttribute = assemb.GetCustomAttribute<ResourceLocationAttribute>();
            }

        }
    }

}
