using Common;
using Common.Attributes;
using Common.Linq;
using DataBase.Base.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class SmartLookupController<TModel> : SmartController<TModel> where TModel : DbSetBase ,new()
    {
        public ActionResult Lookup(string Hids, bool Multi = false, LookupType Lt = LookupType.Base)
        {
            var result = _iRepositoryService.GetAll().Search(Request.QueryString);
            IList Model = null;
            switch (Lt)
            {
                case LookupType.Base:
                    int pagedIndex = 1;
                    if (Request["pagedIndex"] != null) int.TryParse(Request["pagedIndex"].ToString(), out pagedIndex);
                    Model = result.ToPagedList(pagedIndex);
                    break;
                case LookupType.Simple:
                    Model = result.ToList();
                    break;
                default:
                    throw new Exception("Unkonwn Lookup Style!");
            }

            return View("Lookup_" + Enum.GetName(Lt.GetType(), Lt), Model);
        }
    }
}