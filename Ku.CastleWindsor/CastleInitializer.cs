using Castle.Facilities.TypedFactory;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Ku.Main;
using Ku.Main.Commons;
using Ku.Main.Ioc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ku.CastleWindsor
{
    public static class CastleInitializer
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer Container
        {
            get
            {
                return _container;
            }
        }

        public static void Initialize(Action<IWindsorContainer> postAction)
        {
            string path = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)).AbsolutePath;

            _container = new WindsorContainer();
            _container = new WindsorContainer();
            _container.AddFacility<TypedFactoryFacility>();
            _container.Kernel.Resolver.
                AddSubResolver(new CollectionResolver(_container.Kernel));

            _container.Register(
                    Component.
                        For<IIocService>().
                        Instance(new CastleService(_container, path)).
                        LifestyleSingleton()
                );
            var castleStatus = _container.Resolve<IIocService>();

            var filter = new AssemblyFilter(castleStatus.BinPath).
                FilterByAssembly(a => !a.IsDynamic);

            _container.Install(
                FromAssembly.
                    InDirectory(filter));

            _container.Register(
                Classes.
                    FromAssemblyInDirectory(filter).
                    BasedOn<IService>().
                    WithServiceAllInterfaces().
                    LifestyleSingleton(),
                Classes.
                    FromAssemblyInDirectory(filter).
                    BasedOn<IComponent>().
                    WithServiceAllInterfaces().
                    LifestyleTransient()
             );

            var inspectorService = _container.Resolve<IInspectionService>();
            var loggerFactory = inspectorService.GetTypes<ILoggerFactory>().FirstOrDefault();
            if (loggerFactory != null)
            {
                LoggerManager.Initialize(_container.Resolve<ILoggerFactory>());
            }

            if (postAction != null)
            {
                postAction(_container);
            }
        }
    }
}
