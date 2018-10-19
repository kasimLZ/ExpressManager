using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DataForAttribute : Attribute
    {
        public DataForAttribute() { Mode = UpdateMode.None;  }

        /// <summary>
        /// 数据源
        /// </summary>
        public string DataScoure { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Value { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		public string ShowName { get; set; }

        /// <summary>
        /// 更新模式
        /// </summary>
        public UpdateMode Mode { get; set; }
    }

    public enum UpdateMode
    {
        /// <summary>
        /// 不进行外键自动填充
        /// </summary>
        None,

        /// <summary>
        /// 追加，用于多对多关系表
        /// </summary>
        Add,

        /// <summary>
        /// 替换，用于多对多关系表
        /// </summary>
        Replace,

        /// <summary>
        /// 填充，用于一对多对集合外键填充
        /// </summary>
        Filling
    }
}
