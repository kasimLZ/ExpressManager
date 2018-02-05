using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Helper
{
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        public IList<string> AllowAreas { private get; set; }

        public IList<string> AllowController { private get; set; }

        public IList<string> AllowAction { private get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            var area = (string)httpContext.Request.RequestContext.RouteData.DataTokens["area"];
            var action = (string)httpContext.Request.RequestContext.RouteData.Values["action"];
            var controller = (string)httpContext.Request.RequestContext.RouteData.Values["controller"];

            if (AllowAreas.Contains(area) || AllowController.Contains(controller) || AllowAction.Contains(action)) return true;
            else return base.AuthorizeCore(httpContext);
        }
    }
}