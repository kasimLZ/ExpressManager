using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Base.Model
{
    public class SysRoleSysControllerSysAction : DbSetBase
    {

        public virtual SysControllerSysAction SysControllerSysAction { get; set; }

        [ForeignKey("SysControllerSysAction")]
        public long SysControllerSysActionId { get; set; }

        [ForeignKey("SysRole")]
        public long SysRoleId { get; set; }

        public virtual SysRole SysRole { get; set; }

    }



}
