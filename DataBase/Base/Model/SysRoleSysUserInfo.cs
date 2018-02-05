using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Base.Model
{
    public class SysRoleSysUserInfo : DbSetBase
    {
        // Properties
        public virtual SysRole SysRole { get; set; }

        [ForeignKey("SysRole")]
        public long SysRoleId { get; set; }

        public virtual SysUserInfo SysUserInfo { get; set; }

        [ForeignKey("SysUserInfo")]
        public long SysUserId { get; set; }
    }



}
