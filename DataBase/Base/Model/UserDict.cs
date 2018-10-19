using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common.Attributes;
using DataBase.Base.Interface;

namespace DataBase.Base.Model
{
	public class UserDict :DbSetBase
	{
		public UserDict(){
			IsEnable = true;
		}

		/// <summary>
		/// 类型id
		/// </summary>
		[ForeignKey("DictType")]
		[SelectList(typeof(IUserDictTypeInterface), "GetUserDictType")]
        [Display(Name = "类型id")]
        public long DictTypeId { get; set; }

		/// <summary>
		/// 类型
		/// </summary>
		[ScaffoldColumn(false)]
		public virtual UserDictType DictType { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        [Display(Name = "文本")]
        public string Text { get; set; }

		/// <summary>
		/// 值
		/// </summary>
		[Display(Name = "值")]
		public string Value { get; set; }

		/// <summary>
		/// 排序
		/// </summary>
		[Display(Name = "排序")]
		public int Sort { get; set; }

		/// <summary>
		/// 说明
		/// </summary>
		[MaxLength(1000)]
        [Display(Name = "说明")]
		public string Description { get; set; }

		/// <summary>
		/// 启用
		/// </summary>
		[Display(Name = "启用")]
		public bool IsEnable { get; set; }

		/// <summary>
		/// 默认
		/// </summary>
		[Display(Name = "默认")]
		public bool IsDefault { get; set; }
	}
}
