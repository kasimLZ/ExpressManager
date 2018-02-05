using Common;
using Common.Linq;
using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class SmartController<TModel> : BaseController where TModel: DbSetBase, new()
    {
        private readonly IRepositoryBase<TModel> _iRepositoryService = DependencyResolver.Current.GetService<IRepositoryBase<TModel>>();
        protected readonly IUnitOfWork _iUnitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();
        protected readonly ICurrentUser _iCurrentUser = DependencyResolver.Current.GetService<ICurrentUser>();

        protected string _jsModel = string.Empty;
        protected List<string> _searchFields = new List<string>();

        public ActionResult Index(int pagedIndex = 1)
        {
            ViewData["_JsModel"] = _jsModel;
            ViewData["_SearchFields"] = _searchFields;

            var model = _iRepositoryService.GetAll().Search(Request.QueryString).ToPagedList(pagedIndex);
            
            return View(model);
        }

        public ActionResult Edit(long? Id)
        {
            TModel model = new TModel();
            if (Id.HasValue) {
                model = _iRepositoryService.GetById(Id.Value);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(long? Id, TModel model)
        {
            if (!ModelState.IsValid)
            {
                return Edit(Id);
            }
            _iRepositoryService.Save(Id, model);
            _iUnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(long [] ids)
        {
            bool State = true ;
            long Count = 0; 
            try {
                _iRepositoryService.Delete(a => ids.Contains(a.Id));
                Count = _iUnitOfWork.Commit();
            }
            catch {
                State = false;
            }
            return Json(new { Success = State, Records = Count });
        }
    }
}