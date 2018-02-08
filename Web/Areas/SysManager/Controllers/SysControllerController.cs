using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Controllers;
using DataBase.Base.Model;

namespace Web.Areas.SysManager.Controllers
{
    public class SysControllerController : SmartController<SysController>
    {
        public override ActionResult Edit(long? id)
        {
            SysController model = new SysController();
            if (id.HasValue)
                model = _iRepositoryService.GetById(id.Value);
            model.SysActionsId = model.SysControllerSysActions != null ? model.SysControllerSysActions.Select(a => a.SysActionId).ToList() : new List<long>();
            return View(model);
        }
    }
}