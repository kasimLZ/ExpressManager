using DataBase.Base.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Web.Areas.Account.Models;

namespace Web.Areas.Account.Controllers
{
    public class LoginController : Controller
    {
       
        private readonly SysUserInfoInterface _iSysUserService;

        public LoginController(SysUserInfoInterface iSysUserService)
        {
            _iSysUserService = iSysUserService;
        }

        // GET: Account/Login
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (Session["ValidateCode"] != null && Session["ValidateCode"].ToString() != model.ValidateCode)
                return View(new LoginModel { ErrorMsg = "验证码错误" });


            var user = _iSysUserService.GetUser(model.UserName, model.Password);

            if (user == null)
            {
                return View(new LoginModel { ErrorMsg = "用户名密码错误" });
            }

            Dictionary<string, string> UserInfo = new Dictionary<string, string>
            {
                { "UserName", user.RealName },
                { "UserId", user.Id.ToString() },
            };
           

            string ReturnUrl = string.IsNullOrEmpty(Request["ReturnUrl"]) ? FormsAuthentication.DefaultUrl : "/#!" + Request["ReturnUrl"];

            FormsAuthentication.SetAuthCookie(JsonConvert.SerializeObject(UserInfo), model.RememberMe);
            return Redirect(ReturnUrl);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }


    }
}