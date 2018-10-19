using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class SelectListAttribute : DataTypeAttribute, IEnumerableField, ITranslatable
	{
		/// <summary>
		/// 无参设置下拉菜单字段
		/// <para>此方法会尝试从元数据获取菜单内容，无法使用自动获取菜单内容方法</para>
		/// </summary>
		/// <param name="multi">多选</param>
		public SelectListAttribute(bool multi = false)
			: base(multi ? "MultiSelectList" : "SelectList")
		{
			Multi = multi;
		}

		/// <summary>
		/// 设置下拉菜单字段
		/// </summary>
		/// <param name="service">依赖类</param>
		/// <param name="multi">多选</param>
		public SelectListAttribute(Type service, bool multi = false)
			 : base(multi ? "MultiSelectList" : "SelectList")
		{
			ServiceType = service;
			Multi = multi;
		}

		/// <summary>
		/// 设置下拉菜单型字段
		/// </summary>
		/// <param name="service">依赖类</param>
		/// <param name="function">获取列表方法名称</param>
		/// <param name="multi">多选</param>
		public SelectListAttribute(Type service, string function, bool multi = false)
			 : base(multi ? "MultiSelectList" : "SelectList")
		{
			ServiceType = service;
			FunctionOrPropertyName = function;
			Multi = multi;
		}

		/// <summary>
		/// 依赖类型
		/// </summary>
		public Type ServiceType { get; set; }

		/// <summary>
		/// 获取列表方法名称
		/// </summary>
		public string FunctionOrPropertyName { get; set; } = "SelectList";

		/// <summary>
		/// 是否为多选
		/// </summary>
		public bool Multi { get; set; }


		/// <summary>
		/// 
		/// </summary>
		public char Split { get; set; } = ',';

		public string Key { get; set; }
		TranslatMode ITranslatable.Mode { get; set; } = TranslatMode.Dict;

		Type ITranslatable.ModelType { get { return ServiceType; } set { ServiceType = value; } }

		string ITranslatable.Value { get { return FunctionOrPropertyName; } set { FunctionOrPropertyName = value; } }
	}
}
