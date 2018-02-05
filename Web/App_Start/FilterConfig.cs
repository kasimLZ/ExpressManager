using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Web.Helper;

namespace Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new UserAuthorizeAttribute { AllowAreas = new List<string> { "Account" }, AllowController = new List<string> { }, AllowAction = new List<string> { } });
        }
    }
}
