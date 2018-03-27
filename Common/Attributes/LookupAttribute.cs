using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LookupAttribute : DataTypeAttribute
    {
        public LookupAttribute() : base("Lookup")
        {
            LookupType = LookupType.Base;
        }

        /// <summary>
        /// 弹窗类型
        /// </summary>
        public LookupType LookupType { get; set; }

        /// <summary>
        /// 多选
        /// </summary>
        public bool Multi { get; set; }

        /// <summary>
        /// 链接配置
        /// </summary>
        public string LinkConfig { get; set; }

        /// <summary>
        /// 显示字段
        /// </summary>
        public string ShowField { get; set; }

        /// <summary>
        /// 主键字段
        /// </summary>
        public string PrimaryKey { get; set; }
        
        /// <summary>
        /// 筛选条件
        /// </summary>
        public string Condition { get; set; }

        
    }

    public enum LookupType {
        Base,
        [Remark("{width:300,height:200}")]
        Simple
    }
}
