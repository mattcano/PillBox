using Ninject;
using PillBox.Website.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PillBox.Website.DI
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel _kernel;

        public NinjectControllerFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                controllerType = typeof(HomeController);
            }

            return _kernel.Get(controllerType) as IController;
        }
    }
}