using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using Ku.Main;
using Ku.Main.Ioc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ku.CastleWindsor
{
    public class CastleService : IIocService
    {
        private static IWindsorContainer _container;
        private Lazy<HashSet<Type>> _potentiallyMisconfigured;
        private ConcurrentDictionary<Type, String> _unresolvedTypes;
        private ConcurrentDictionary<Type, Lazy<List<PropertyInfo>>> _typesCache;
        private string _path;

        internal CastleService(IWindsorContainer container,string path)
        {
            _path = path;
            _container = container;
            _unresolvedTypes = new ConcurrentDictionary<Type, String>();
            _typesCache = new ConcurrentDictionary<Type, Lazy<List<PropertyInfo>>>();
            _potentiallyMisconfigured = new Lazy<HashSet<Type>>(() =>
            {
                var host = (IDiagnosticsHost)_container.Kernel.GetSubSystem(SubSystemConstants.DiagnosticsKey);
                var diagnostics = host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>();
                var result = diagnostics.Inspect().Select(k => k.ComponentModel.Implementation);
                return new HashSet<Type>(result);
            });
        }

        public T ResolveProperties<T>(T target)
        {
            if (target == null) return target;

            ;

            foreach (var property in _typesCache.GetOrAdd(typeof(T), new Lazy<List<PropertyInfo>>(() =>
            {
                return typeof(T).GetProperties().Where(p => p.CanWrite && p.CanRead).ToList();
            })).Value)
            {
                if (_unresolvedTypes.ContainsKey(property.PropertyType)) continue;
                try
                {
                    if (property.GetValue(target) == null)
                    {
                        if (_container != null
                            && _container.Kernel.HasComponent(property.PropertyType)
                            && !_potentiallyMisconfigured.Value.Contains(property.PropertyType))
                        {
                            property.SetValue(target, _container.Resolve(property.PropertyType));
                        }
                    }
                }
                catch (Exception)
                {
                    _unresolvedTypes[property.PropertyType] = target.GetType().FullName;
                }
            }
            return target;
        }

        public HashSet<Type> PotentiallyMisconfigured
        {
            get
            {
                return _potentiallyMisconfigured.Value;
            }
        }



        public string BinPath
        {
            get { return _path; }
        }


        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
