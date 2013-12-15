using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Forum
{
    public class NonvalidatedInputControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {

            var controller = base.GetControllerInstance(requestContext,
                controllerType);

            string validateRequest = System.Configuration.ConfigurationManager.AppSettings["validateRequest"];

            bool b;

            if (validateRequest != null && bool.TryParse(validateRequest, out b))

                ((ControllerBase)controller).ValidateRequest = bool.Parse(validateRequest);

            return controller;
        }
    }
}