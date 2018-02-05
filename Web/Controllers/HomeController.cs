using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICurrentUser _iCurrentUser;

        public HomeController(ICurrentUser iCurrentUser)
        {
            _iCurrentUser = iCurrentUser;
        }

        public ActionResult Index()
        {
            return View(_iCurrentUser);
        }

    }
}