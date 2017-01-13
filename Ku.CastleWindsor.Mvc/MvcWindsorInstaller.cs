using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ku.CastleWindsor.WebMvc
{
    public class MvcWindsorInstaller:IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            ControllerBuilder.Current.
                SetControllerFactory(new WindsorMvcControllerFactory(container));
        }
    }
}
