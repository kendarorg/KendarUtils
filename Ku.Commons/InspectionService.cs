using Ku.Main.Commons;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ku.Commons
{
    public class InspectionService : IInspectionService
    {
        private static ConcurrentDictionary<Type, ReadOnlyCollection<Type>> _concreteTypes = new ConcurrentDictionary<Type, ReadOnlyCollection<Type>>();
        private static ConcurrentDictionary<Type, ReadOnlyCollection<Type>> _abstractTypes = new ConcurrentDictionary<Type, ReadOnlyCollection<Type>>();
        private static ConcurrentDictionary<Type, ReadOnlyCollection<Type>> _interfaceTypes = new ConcurrentDictionary<Type, ReadOnlyCollection<Type>>();
        public IEnumerable<Type> GetTypes<T>(InspectedType inspectedType = InspectedType.Concrete)
        {
            var type = typeof(T);
            var asms = AppDomain.CurrentDomain.GetAssemblies();

            if ((inspectedType & InspectedType.Concrete) == InspectedType.Concrete)
            {
                foreach (var item in _concreteTypes.GetOrAdd(type, (t) => GetTypes(asms, t, InspectedType.Concrete))) yield return item;
            }
            if ((inspectedType & InspectedType.Abstract) == InspectedType.Abstract)
            {
                foreach (var item in _abstractTypes.GetOrAdd(type, (t) => GetTypes(asms, t, InspectedType.Abstract))) yield return item;
            }
            if ((inspectedType & InspectedType.Interface) == InspectedType.Interface)
            {
                foreach (var item in _abstractTypes.GetOrAdd(type, (t) => GetTypes(asms, t, InspectedType.Interface))) yield return item;
            }
        }

        private static ReadOnlyCollection<Type> GetTypes(Assembly[] asms, Type t, InspectedType inspectedType)
        {
            return new ReadOnlyCollection<Type>(
                asms
                    .Where((asm) => !asm.IsDynamic)
                    .SelectMany(s => s.GetTypes())
                    .Where(p =>
                    {
                        if (!p.IsAssignableFrom(p)) return false;

                        if ((inspectedType & InspectedType.Concrete) == InspectedType.Concrete)
                        {
                            return !p.IsInterface && !p.IsAbstract;
                        }
                        else if ((inspectedType & InspectedType.Abstract) == InspectedType.Abstract)
                        {
                            return !p.IsInterface && p.IsAbstract;
                        }
                        else if ((inspectedType & InspectedType.Interface) == InspectedType.Interface)
                        {
                            return p.IsInterface && !p.IsAbstract;
                        }
                        return false;
                    }).ToList()
                );
        }
    }
}
