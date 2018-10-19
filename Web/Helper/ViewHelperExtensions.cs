using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using Common;
using System.Collections.Specialized;
using System.Text;
using System.Globalization;
using Common.Linq;
using DataBase.Base.Model;
using DataBase.Base.Interface;
using Common.Attributes;
using DataBase.Base.Infrastructure.Interface;
using Web.Helper;

namespace Web.Helpers
{
    public static class ViewHelperExtensions
    {
        /// <summary>
        /// Obtain the field name based on the field properties and try to translate the field
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static string GetLabel(this PropertyInfo propertyInfo)
        {
            var meta = ModelMetadataProviders.Current.GetMetadataForProperty(null, propertyInfo.DeclaringType, propertyInfo.Name);
            return meta.GetDisplayName();
        }

		public static IEnumerable<SelectListItem> StuffSelectViewData(this ModelMetadata prop, object select = null)
		{
			return StuffSelectViewData(prop.ContainerType.GetProperty(prop.PropertyName), select);
		}


		public static IEnumerable<SelectListItem> StuffSelectViewData(this PropertyInfo prop, object select = null)
        {
            var sa = prop.GetCustomAttribute<SelectListAttribute>();
			var service = DependencyResolver.Current.GetService(sa.ServiceType);
            var ParamTyps = new List<Type>();
            if (select != null)
                ParamTyps.Add(select.GetType());
            else
                ParamTyps.Add(typeof(IEnumerable));
            var mothed = sa.ServiceType.GetMethod(sa.FunctionOrPropertyName,BindingFlags.Public | BindingFlags.Instance, null, ParamTyps.ToArray(), null);
            if (mothed != null)
                try { return mothed.Invoke(service, new object [] { select }) as IEnumerable<SelectListItem>; } catch { }
            return null;
        }
		
		public static IEnumerable<SelectListItem> StuffUserDictViewData(this ModelMetadata prop, object value = null)
		{
			return StuffUserDictViewData(prop.ContainerType.GetProperty(prop.PropertyName), value);
		}

		public static IEnumerable<SelectListItem> StuffUserDictViewData(this PropertyInfo prop, object value = null)
		{
			IUserDictInterface _IUserDictService = DependencyResolver.Current.GetService<IUserDictInterface>();
			UserDictAttribute Info = prop.GetCustomAttribute<UserDictAttribute>();

			if (!Info.Multi)
			{
				return _IUserDictService.SelectListByType(Info.DictType, value);
			}
			else
			{
				IEnumerable select;
				if (value is string)
				{
					select = value.ToString().Split(Info.Split);
				}
				else
				{
					select = value as IEnumerable;
				}

				return _IUserDictService.SMultiSelectListByType(Info.DictType, select);
			}
		}

		public static bool IsRequiredField(this ModelMetadata prop)
		{
			return (prop.IsRequired && prop.ModelType != typeof(bool));
		}

		/// <summary>
		/// Separated word by space
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ToSeparatedWords(this string value)
        {
            return Regex.Replace(value, "([A-Z][a-z])", " $1").Trim();
        }

        /// <summary>
        /// Try looking for the primary key field in the object
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string IdentifierPropertyName(this Object model)
        {
            return TypeExtensions.IdentifierPropertyName(model is Type ? model as Type : model.GetType());
        }
        
        public static Type GetElementType(this IEnumerable Model, int GenericArgument = 0)
        {
            var elementType = Model.GetType().GetElementType();
            if (elementType == null)
            {
                elementType = Model.GetType().GetGenericArguments()[GenericArgument];
            }
            return elementType;
        }

		public static Type GetElementType(this PropertyInfo Model, int GenericArgument = 0)
		{
			var elementType = Model.PropertyType.GetElementType();
			if (elementType == null)
			{
				elementType = Model.PropertyType.GetGenericArguments()[GenericArgument];
			}
			return elementType;
		}

		/// <summary>
		/// All visual fields in the model
		/// </summary>
		/// <param name="Model"></param>
		/// <returns></returns>
		public static PropertyInfo[] VisibleProperties(this IEnumerable Model, IEnumerable<string> HiddenField = null, IEnumerable<string> ShowField = null)
        {
            var elementType = Model.GetType().GetElementType();
            if (elementType == null)
            {
                elementType = Model.GetType().GetGenericArguments()[0];
            }
            return elementType.GetProperties()
                        .Where(a => !a.GetAccessors().Any(b => b.IsVirtual) || ShowField.Contains(a.Name))
                        .Where(a => !HiddenField.Contains(a.Name)).OrderedByDisplayAttr().ToArray();
            
        }

        public static IOrderedEnumerable<PropertyInfo> OrderedByDisplayAttr(this IEnumerable<PropertyInfo> collection)
        {
            return collection.OrderBy(col =>
            {
                var attr = col.GetAttribute<DisplayAttribute>();
                return (attr != null ? attr.GetOrder() : null) ?? 0;
            });
        }

        public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : class
        {
            return propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
        }
        
        public static object CreateInstance(this Type type, NameValueCollection collection = null)
        {
            object Instance = type != typeof(string) ? Activator.CreateInstance(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) ? type.GetGenericArguments()[0] : type) : string.Empty;
            if (collection != null)
            {
                PropertyInfo[] Properties = type.GetProperties();
                foreach (string key in collection.Keys)
                {
                    var info = Properties.FirstOrDefault(a => a.Name.Equals(key));
                    if (info != null && !string.IsNullOrEmpty(collection[key]))
                        info.SetValue(Instance, Convert.ChangeType(collection[key], info.PropertyType));
                }
            }
            return Instance;
        }

        public static Expression<Func<T, R>> EditLambda<T,R>(this PropertyInfo Info, object model)
        {
            return Expression.Lambda<Func<T, R>>(Expression.PropertyOrField(Expression.Constant(model), Info.Name), Expression.Parameter(typeof(T), "a"));
        }

		public static ModelMetadata GetPropMetadata(this HtmlHelper html)
		{
			return html.ViewData.ContainsKey("ModelMetadata") ? html.ViewData["ModelMetadata"] as ModelMetadata : html.ViewData.ModelMetadata;
		}
        

        public static String HashPager<T>(this HtmlHelper html, IPagedList<T> data, object args = null)
        {
            return html.HashPager(data.PageIndex, data.PageSize, data.TotalCount, args);
        }

        public static String HashPager(this HtmlHelper html, int pageIndex, int pageSize, int totalCount, object args = null)
        {
            var totalPage = (int)Math.Ceiling((double)totalCount / pageSize);
            List<int> pageNumber = new List<int>();
            for (int i = 1; i <= 3; i++) pageNumber.Add(i);
            for (int i = totalPage - 2; i <= totalPage; i++) pageNumber.Add(i);
            for (int i = pageIndex - 3; i <= pageIndex + 3; i++) pageNumber.Add(i);
            pageNumber = pageNumber.Where(a => a > 0 && a <= totalPage).Distinct().OrderBy(a => a).ToList();


            RouteValueDictionary vs = html.ViewContext.RouteData.Values;

            NameValueCollection queryString = html.ViewContext.HttpContext.Request.QueryString;
            foreach (string key in queryString.Keys)
                vs[key] = queryString[key];

            NameValueCollection formString = html.ViewContext.HttpContext.Request.Form;
            foreach (string key in formString.Keys)
                vs[key] = formString[key];

            if (args != null)
            {
                foreach (PropertyInfo property in args.GetType().GetProperties())
                {
                    string name = property.Name;
                    string value = property.GetValue(args, null).ToString();
                    vs[name] = value;
                }
            }

            vs.Remove("X-Requested-With");
            vs.Remove("X-HTTP-Method-Override");
            vs.Remove("_tab");

            var builder = new StringBuilder();
            builder.AppendFormat("<div class=\"row\"><div class=\"col-md-4\" style=\"margin-top:20px\">");
            int start = pageSize * (pageIndex - 1) + 1, stop = pageSize * pageIndex;
            builder.Append(string.Format("显示 {0} 到 {1} 项，共 {2} 项", start, stop > totalCount ? totalCount : stop, totalCount));
            builder.AppendFormat("</div><div class=\"col-md-8\"><ul class=\"pull-right pagination\">");

            bool Enable = pageIndex > 1;
            builder.Append(html.PagerItem("首页", Enable ? vs["action"].ToString() : null, vs, 1));
            builder.Append(html.PagerItem("上一页", Enable ? vs["action"].ToString() : null, vs, pageIndex - 1));

            for (int i = 0; i < pageNumber.Count; i++)
            {
                var index = pageNumber[i];
                if (i > 1 && index > pageNumber[i - 1] + 1)
                    builder.Append(html.PagerItem("...", null, null));
                builder.Append(html.PagerItem(index.ToString(CultureInfo.InvariantCulture), index != pageIndex ? vs["action"].ToString() : null, vs, index));
            }

            Enable = (pageIndex * pageSize) < totalCount;
            builder.Append(html.PagerItem("下一页", Enable ? vs["action"].ToString() : null, vs, pageIndex + 1));
            builder.Append(html.PagerItem("末页", Enable ? vs["action"].ToString() : null, vs, totalPage));

            builder.Append("</ul></div></div>");
            return builder.ToString();
        }

        private static MvcHtmlString PagerItem(this HtmlHelper html, string text, string action, RouteValueDictionary vs, int? page = null, string cssClass = null)
        {
            var item = new TagBuilder("li");

            if (string.IsNullOrEmpty(action))
            {
                var a = new TagBuilder("a");
                a.SetInnerText(text);
                a.Attributes["href"] = "javascript:;";
                a.AddCssClass(cssClass);
                item.InnerHtml += a;
            }
            else
            {
                vs["pagedIndex"] = page.HasValue ? page.Value : 1;
                item.InnerHtml += html.ActionLink(text, vs["action"].ToString(), vs, new Dictionary<string, object> { { "data-goto" , null } });
                vs.Remove("pageIndex");
            }

            return new MvcHtmlString(item.ToString());
        }

        public static IEnumerable<SysAction> GetUserButtons(this HtmlHelper htmlHelper, string Controller = "")
        {
            var _iCurrentUser = DependencyResolver.Current.GetService<ICurrentUser>();
            var _iSysActionService = DependencyResolver.Current.GetService<ISysActionInterface>();
			var buttons = _iSysActionService.GetAll(
				a => a.SysControllerSysActions.Any(
					b => b.SysRoleSysControllerSysActions.Any(
						c => c.SysRole.SysRoleSysUsers.Any(
							d => d.SysUserId == _iCurrentUser.Id
							)
						)
					) && (a.ButtonType != ButtonTypes.None)
				 );
			if (!string.IsNullOrEmpty(Controller)) {
				buttons = buttons.Where(a => a.SysControllerSysActions.Any(b => b.SysController.ControllerName == Controller));
			}
			return buttons.ToList();
        }

        public static MvcHtmlString RenderToolbar(this UrlHelper urlHelper, IEnumerable<SysAction> Buttons)
        {

            var row = new TagBuilder("div");
            row.AddCssClass("row");
            var div = new TagBuilder("div");
            div.AddCssClass("col-md-12 content-toolbar");
            if (Buttons != null)
            {
                foreach (var btn in Buttons)
                {
                    div.InnerHtml += urlHelper.CreateButtonItem(btn);
                }
            }
            row.InnerHtml += div;
            return new MvcHtmlString(row.ToString());
        }

        public static MvcHtmlString RenderInlineButton(this UrlHelper urlHelper, ICollection<SysAction> Buttons, object id)
        {
            StringBuilder bulider = new StringBuilder();
            var area = (string)System.Web.HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
            if (Buttons != null)
            {
                foreach (var btn in Buttons)
                {
                    bulider.Append(urlHelper.CreateButtonItem(btn, id, area).ToString());
                }
            }

            return new MvcHtmlString(bulider.ToString());
        }

        private static TagBuilder CreateButtonItem(this UrlHelper urlHelper, SysAction button, object id = null, string Area = null)
        {
            var Button = new TagBuilder("button");
            Button.Attributes["type"] = "button";
            Button.AddCssClass("btn btn-sm");
            if (!string.IsNullOrEmpty(button.ButtonStyle))
                Button.AddCssClass("btn-" + button.ButtonStyle);
            if (!string.IsNullOrEmpty(button.ButtonIcon))
            {
                var icon = new TagBuilder("i");
                icon.AddCssClass("margin-right-5 " + button.ButtonIcon);
                Button.InnerHtml += icon;
            }
            Button.InnerHtml += button.ActionDisplayName;

            switch (button.ActionType)
            {
                case ActionTypes.Default:
                case ActionTypes.Event:
                    Button.Attributes["onclick"] = id != null ?
                        "Core.BtnAction(\"" + button.ActionName + "\", \"" + id.ToString() + "\")" :
                        "Core.BtnAction(\"" + button.ActionName + "\")";
                    break;
                case ActionTypes.Link:
                    Button.Attributes["data-goto"] =  urlHelper.Action(button.ActionName, new { id });
                    break;
            }


            return Button;
        }

        /// <summary>
        /// 根据当前用户权限生成菜单
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString CreateMenuList(this HtmlHelper htmlHelper, UrlHelper Url, ICurrentUser user)
        {
            var _iSysController = DependencyResolver.Current.GetService<ISysControllerInterface>();

            var Controller = _iSysController.GetAll(
                a => a.Display && a.SysControllerSysActions.Any(
                    b => !b.Deleted && b.SysRoleSysControllerSysActions.Any(
                        c => !c.Deleted && c.SysRole.SysRoleSysUsers.Any(
                            d => !d.Deleted && d.SysUserId.Equals(user.Id.Value)
                        )
                    )
                )
            ).OrderBy(a => a.SystemId).ToList();

            var Code = "000";
            StringBuilder bulider = new StringBuilder();
            
            foreach (var item in Controller) {
                if (item.SystemId.Length < Code.Length)
                {
                    bulider.Append("</ul></li>");
                }
                bulider.Append("<li>");

                var a = new TagBuilder("a");
                
                if (!string.IsNullOrEmpty(item.Ico))
                {
                    a.InnerHtml += "<i class=\"fa " + item.Ico + "\"></i>";
                }
                a.InnerHtml += (item.SystemId.Length > 3) ? item.ControllerDisplayName : "<span class=\"nav-label\">" + item.ControllerDisplayName + "</span>";

                if (item.SystemId.Length < 9 && Controller.Any(b => b.SystemId.StartsWith(item.SystemId) && b.SystemId.Length == (item.SystemId.Length + 3)))
                {
                    a.Attributes["href"] = "javascript:;";
                    a.InnerHtml += "<span class=\"fa arrow\"></span>";
                    bulider.Append(a.ToString());
                    bulider.Append("<ul class=\"nav nav-" + (item.SystemId.Length == 3 ? "second" : "third") + "-level collapse\">");
                }
                else
                {
                    
                    a.Attributes["data-goto"] = a.Attributes["href"] = "/" + item.SysArea.AreaName + Url.Action(item.ActionName, item.ControllerName);
                    bulider.Append(a.ToString());
                    bulider.Append("</li>");
                }
                Code = item.SystemId;
            }

            for (int i = 0; i < (Code.Length / 3) - 1; i++)
            {
                bulider.Append("</ul></li>");
            }
            
            return new MvcHtmlString(bulider.ToString());
        }

		public static string RouteScope(this HtmlHelper htmlHelper)
		{
			return string.Format("{0}#{1}#{2}", htmlHelper.ViewContext.RouteData.Values["area"], htmlHelper.ViewContext.RouteData.Values["controller"], htmlHelper.ViewContext.RouteData.Values["index"]).ToMD5();
		}

		public static void ViewDictForModel(this HtmlHelper html, IEnumerable list, IEnumerable<string> names = null)
		{
			var model = GetElementType(list).CreateInstance();
			if (names == null) names = new string[0];
			foreach (var prop in model.GetType().GetProperties().Where(a => !names.Contains(a.Name) && a.CustomAttributes.Any(b => typeof(ITranslatable).IsAssignableFrom(b.AttributeType))))
			{
				if (html.ViewData[prop.Name] == null)
				{
					var Trans = prop.GetAttribute<ITranslatable>();
					Type type = (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string)) ? GetElementType(prop) : prop.PropertyType;
					
					html.ViewData[prop.Name] = prop.GetViewDict(ValueList(Trans, list, type.CreateInstance() as dynamic, prop) as object);
				}
			}
		}
		
		private static  List<T> ValueList<T>(ITranslatable trans, IEnumerable model , T _value, PropertyInfo info)
		{
			List<T> list = new List<T>();
			foreach (var item in model)
			{
				var value = info.GetValue(item);
				if (trans is IEnumerableField)
				{
					var ef = trans as IEnumerableField;
					if (value is string && ef.Multi)
						list.AddRange(value.ToString().Split(ef.Split).ConvertIEnumerable<T>());
					else if (item is IEnumerable)
						list.AddRange((item as IEnumerable).ConvertIEnumerable<T>());
					else
						try { list.Add((T)item); } catch { }
					
				}
				else
				{
					try { list.Add((T)item); } catch { }
				}
			}
			return list.Distinct().ToList();
		}

		public static string LookupUrl(this UrlHelper url, LookupAttribute lookup, string lookupId)
		{
			string LookUpUrl = string.Empty;
			switch (lookup.LinkModel)
			{
				case LookupLink.None: break;
				case LookupLink.Link: LookUpUrl = lookup.LinkConfig as string ?? string.Empty; break;
				case LookupLink.Route:
					if (lookup.LinkConfig is string)
					{
						var routeValue = new RouteValueDictionary(
							lookup.LinkConfig.ToString().Split(',')
							.Select(a => { var b = a.Split('='); return new KeyValuePair<string, object>(b[0], b[1]); })
							.ToDictionary(k => k.Key, v => v.Value)
							);
						LookUpUrl = url.RouteUrl(routeValue);
					}
					else
					{
						LookUpUrl = url.RouteUrl(lookup.LinkConfig);
					}

					break;
			}
			if (!string.IsNullOrEmpty(LookUpUrl))
			{
				LookUpUrl += string.Format("?Hids={0}&Multi={1}&Style={2}", lookupId, lookup.Multi, lookup.LookupModel);

				if (!string.IsNullOrEmpty(lookup.Condition))
				{
					LookUpUrl += "&" + lookup.Condition;
				}
			}
			return LookUpUrl;
		}

	}
}