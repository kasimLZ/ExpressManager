using DataBase.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Web.Controllers;

namespace Web.Areas.SysManager.Controllers
{
    public class SysAreaController : SmartController<SysArea>
    {
        public ActionResult Lookup() {

            return View();
        }
    }
}