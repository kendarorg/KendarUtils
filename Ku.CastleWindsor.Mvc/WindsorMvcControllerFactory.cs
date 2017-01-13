using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ku.CastleWindsor.WebMvc
{
    public class WindsorMvcControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer container;

        public WindsorMvcControllerFactory(IWindsorContainer container)
        {
            this.container = container;
        }

        public override void ReleaseController(IController controller)
        {
            container.Release(controller.GetType());
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, requestContext.HttpContext.Request.Path);
            }
            return (IController)container.Resolve(controllerType);
        }
    }
}
