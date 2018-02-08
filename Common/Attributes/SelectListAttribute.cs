using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    public class SelectListAttribute : DataTypeAttribute
    {
        private bool _multi;
        private Type _serviceType;
        private string _func = "SelectList";

        /// <summary>
        /// 无参设置下拉菜单字段
        /// <para>此方法会尝试从元数据获取菜单内容，无法使用自动获取菜单内容方法</para>
        /// </summary>
        /// <param name="multi">多选</param>
        public SelectListAttribute(bool multi = false)
            : base(multi ? "MultiSelectList" : "SelectList")
        {
            _multi = multi;
        }

        /// <summary>
        /// 设置下拉菜单字段
        /// </summary>
        /// <param name="service">依赖类</param>
        /// <param name="multi">多选</param>
        public SelectListAttribute(Type service, bool multi = false)
             : base(multi ? "MultiSelectList" : "SelectList")
        {
            _serviceType = service;
            _multi = multi;
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
            _serviceType = service;
            _func = function;
            _multi = multi;
        }

        /// <summary>
        /// 依赖类型
        /// </summary>
        public Type ServiceType { get { return _serviceType; } }

        /// <summary>
        /// 获取列表方法名称
        /// </summary>
        public string FunctionName { get { return _func; } }

        /// <summary>
        /// 是否为多选
        /// </summary>
        public bool IsMulti { get { return _multi; } }
    }
}
