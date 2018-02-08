using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Base.Model
{
    public class SysAction : DbSetBase
    {
        public SysAction()
        {
            SystemId = "000";
            ButtonIcon = string.Empty;
            ButtonStyle = string.Empty;
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        [MaxLength(40), Required]
        public string ActionDisplayName { get; set; }

        /// <summary>
        /// 操作名称
        /// </summary>
        [MaxLength(40), Required]
        public string ActionName { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public ActionTypes ActionType { get; set; }

        /// <summary>
        /// 按钮图标
        /// </summary>
        [DataType("Ico")]
        [MaxLength(50)]
        public string ButtonIcon { get; set; }

        /// <summary>
        /// 按钮样式
        /// </summary>
        [MaxLength(50)]
        public string ButtonStyle { get; set; }

        /// <summary>
        /// 按钮类型
        /// </summary>
        public ButtonTypes ButtonType { get; set; }

        /// <summary>
        /// 引用控制器
        /// </summary>
        [ScaffoldColumn(false)]
        public virtual ICollection<SysControllerSysAction> SysControllerSysActions { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [MaxLength(50), Required]
        public string SystemId { get; set; }

        /// <summary>
        /// 可否匿名访问
        /// </summary>
        public bool Anonymous { get; set; }
    }
}
