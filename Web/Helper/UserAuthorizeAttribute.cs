using Common;
using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using System.Web.Mvc;

namespace Web.Helper
{
	public class UserAuthorizeAttribute : AuthorizeAttribute
	{
		protected IList<string> allow_areas = new string [0],
									  allow_controller = new string [0],
									  allow_action = new string [0];

		public IList<string> AllowAreas
		{
			get { var arr = allow_areas.ToList(); arr.AddRange(UserAuthorizeConfigration.AllowAreas); return arr; }
			set { allow_areas = value; }
		}

		public IList<string> AllowController
		{
			get { var arr = allow_controller.ToList(); arr.AddRange(UserAuthorizeConfigration.AllowController); return arr; }
			set { allow_controller = value; }
		}

		public IList<string> AllowAction
		{
			get { var arr = allow_action.ToList(); arr.AddRange(UserAuthorizeConfigration.AllowAction); return arr; }
			set { allow_action = value; }
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var area = (string)httpContext.Request.RequestContext.RouteData.DataTokens["area"];
			var action = (string)httpContext.Request.RequestContext.RouteData.Values["action"];
			var controller = (string)httpContext.Request.RequestContext.RouteData.Values["controller"];

			//可匿名访问里列表
			if (AllowAreas.Contains(area?.ToLower()) || AllowController.Contains(controller?.ToLower()) || AllowAction.Contains(action?.ToLower())) return true;

			//登陆状态检测
			if (!httpContext.User.Identity.IsAuthenticated) return false;

			//默认路径检测，待修改为路由处理
			if (area == null && controller.Equals("home", StringComparison.CurrentCultureIgnoreCase) && action.Equals("index", StringComparison.CurrentCultureIgnoreCase)) return true;


			ISysUserInfoInterface roleSysUserInfoInterface = DependencyResolver.Current.GetService<ISysUserInfoInterface>();
			ICurrentUser currentUser = DependencyResolver.Current.GetService<ICurrentUser>();

			bool authed = roleSysUserInfoInterface.CheckRole(currentUser.Id.Value, area, controller, action);

			if (!authed && httpContext.Request.IsAjaxRequest())
			{
				throw new HttpException(420, "没有访问权限");
			}
			//检测用户权限
			return authed;
		}
	}

	public static class UserAuthorizeConfigration
	{
		

		static UserAuthorizeConfigration()
		{
			RefreshAll();

			Timer timer = new Timer
			{
				Interval = 60 * 60 * 1000,
				Enabled = true,
				AutoReset = true
			};
			timer.Elapsed += new ElapsedEventHandler((object sender, ElapsedEventArgs e) => { Expired = true; });
			timer.Start();
		}

		public static bool Expired = true;

		private static IEnumerable<string> _allowAreas_ { get; set; }

		private static IEnumerable<string> _allowController_ { get; set; }

		private static IEnumerable<string> _allowAction_ { get; set; }

		public static IEnumerable<string> AllowAreas { get { if (Expired) RefreshAll(); return _allowAreas_; } set { _allowAreas_ = value; } }

		public static IEnumerable<string> AllowController { get { if (Expired) RefreshAll(); return _allowController_; } set { _allowController_ = value; } }

		public static IEnumerable<string> AllowAction { get { if (Expired) RefreshAll(); return _allowAction_; } set { _allowAction_ = value; } }

		public static IEnumerable<string> RefreshAction()
		{
			ISysActionInterface sysActionInterface = DependencyResolver.Current.GetService<ISysActionInterface>();
			return AllowAction = sysActionInterface.GetAll(a => a.Anonymous).Select(a => a.ActionName.ToLower()).ToArray();
		}

		[Obsolete]
		public static async void RefreshActionAsync()
		{
			//AllowAction = await Task.Factory.StartNew(RefreshAction);
		}

		public static IEnumerable<string> RefreshContrller()
		{
			ISysControllerInterface sysControllerInterface = DependencyResolver.Current.GetService<ISysControllerInterface>();
			return AllowController = sysControllerInterface.GetAll(a => false).Select(a => a.ActionName.ToLower()).ToArray();
		}

		[Obsolete]
		public static async void RefreshContrllerAsync()
		{
			///AllowController = await Task.Factory.StartNew(RefreshContrller);
		}

		public static IEnumerable<string> RefreshArea()
		{
			ISysAreaInterface sysAreaInterface = DependencyResolver.Current.GetService<ISysAreaInterface>();
			return AllowAreas = sysAreaInterface.GetAll(a => a.Anonymous).Select(a => a.AreaName.ToLower()).ToArray();
		}

		[Obsolete]
		public static async void RefreshAreaAsync()
		{
			//AllowAreas = await Task.Factory.StartNew(RefreshArea);
		}

		public static void RefreshAll()
		{
			RefreshAction();
			RefreshContrller();
			RefreshArea();
			Expired = false;
		}

		[Obsolete]
		public static void RefreshAllAsync()
		{
			RefreshActionAsync();
			RefreshContrllerAsync();
			RefreshAreaAsync();
		}
	}
}