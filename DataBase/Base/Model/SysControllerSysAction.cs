using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Base.Model
{
    public class SysControllerSysAction : DbSetBase
    {
        public virtual SysAction SysAction { get; set; }

        [ForeignKey("SysAction")]
        public long SysActionId { get; set; }

        public virtual SysController SysController { get; set; }

        [ForeignKey("SysController")]
        public long SysControllerId { get; set; }

        public virtual ICollection<SysRoleSysControllerSysAction> SysRoleSysControllerSysActions { get; set; }
    }
}
