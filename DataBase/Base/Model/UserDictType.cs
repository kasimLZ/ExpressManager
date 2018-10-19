using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Base.Model
{
	public class UserDictType : DbSetBase
	{
		/// <summary>
		/// 字典类型
		/// </summary>
		[Display(Name = "字典类型")]
		public string DictType { get; set; }

		/// <summary>
		/// 字典名称
		/// </summary>
		[Display(Name = "字典名称")]
		public string DictTypeName { get; set; }
		
		/// <summary>
		/// 说明
		/// </summary>
		[MaxLength(1000)]
        [Display(Name = "说明")]
		public string Description { get; set; }
		
		/// <summary>
		/// 是否启用
		/// </summary>
		[Display(Name = "是否启用")]
		public bool IsEnable { get; set; }

        /// <summary>
        /// 用户字典ID
        /// </summary>
        [ScaffoldColumn(false)]
        public List<long> UserDictId { get; set; }

		/// <summary>
		/// 用户字典
		/// </summary>
		[ScaffoldColumn(false)]
		public virtual ICollection<UserDict> UserDicts { get; set; }
	}
}
