using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Ku.Main;
using Ku.Main.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Ku.CastleWindsor.Web
{
    public class WebApiControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var castleStatus = container.Resolve<IIocService>();

            var filter = new AssemblyFilter(castleStatus.BinPath).
                FilterByAssembly(a => !a.IsDynamic);

            container.Register(
                Classes.
                    FromAssemblyInDirectory(filter).
                    BasedOn<IHttpController>(). //Web API
                    If(c => c.Name.EndsWith("Controller")).
                    LifestyleTransient()
                );

            GlobalConfiguration.Configuration.Services.Replace(
                typeof(IHttpControllerActivator),
                new WindsorWebApiControllerActivator(container));
        }
    }
}
