using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Base.Model
{
    public class SysArea : DbSetBase
    {
        /// <summary>
        /// 区域中文名称
        /// </summary>
        public string AreaDisplayName { get; set; }

        /// <summary>
        /// 区域名称（项目文件夹）
        /// </summary>
        public string AreaName { get; set; }
        
        /// <summary>
        /// 控制器外键
        /// </summary>
        public virtual ICollection<SysController> SysControllers { get; set; }
        
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 可否匿名访问
        /// </summary>
        public bool Anonymous { get; set; }
    }
}
