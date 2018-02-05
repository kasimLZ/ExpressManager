using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Controllers
{
    public class BaseController : Controller
    {
        public ContentResult AlertMessage(string message)
        {
            return Content(!string.IsNullOrEmpty(message) ? "<script>Core.alert('" + message + "');" : "");
        }

        public ContentResult RedirectUrlScript(string url, string message = null)
        {
            url = url.Replace("#", "");
            return Content("<script>" + (!string.IsNullOrEmpty(message) ? "Core.alert('" + message + "');" : "") + "Core.goToUrl('" + url + "');</script>");
        }
        public new ContentResult RedirectToAction(string actionName)
        {
            string url = Url.Action(actionName);
            return RedirectUrlScript(url);
        }
        public new ContentResult RedirectToAction(string actionName, object routeValues)
        {
            string url = Url.Action(actionName, routeValues);
            return RedirectUrlScript(url);
        }
        public new ContentResult RedirectToAction(string actionName, RouteValueDictionary routeValues)
        {
            string url = Url.Action(actionName, routeValues);
            return RedirectUrlScript(url);
        }
        public new ContentResult RedirectToAction(string actionName, string controllerName)
        {
            string url = Url.Action(actionName, controllerName);
            return RedirectUrlScript(url);
        }
        public new ContentResult RedirectToAction(string actionName, string controllerName, object routeValues)
        {
            string url = Url.Action(actionName, controllerName, routeValues);
            return RedirectUrlScript(url);
        }

        public RedirectToRouteResult RedirectToResult(string actionName)
        {
            return base.RedirectToAction(actionName);
        }

        // 弹框 

        public ContentResult AlertToAction(string message, string actionName)
        {
            string url = Url.Action(actionName);
            return RedirectUrlScript(url, message);
        }
        public ContentResult AlertToAction(string message, string actionName, object routeValues)
        {
            string url = Url.Action(actionName, routeValues);
            return RedirectUrlScript(url, message);
        }
        public ContentResult AlertToAction(string message, string actionName, RouteValueDictionary routeValues)
        {
            string url = Url.Action(actionName, routeValues);
            return RedirectUrlScript(url, message);
        }
        public ContentResult AlertToAction(string message, string actionName, string controllerName)
        {
            string url = Url.Action(actionName, controllerName);
            return RedirectUrlScript(url, message);
        }
        public ContentResult AlertToAction(string message, string actionName, string controllerName, object routeValues)
        {
            string url = Url.Action(actionName, controllerName, routeValues);
            return RedirectUrlScript(url, message);
        }
    }
}