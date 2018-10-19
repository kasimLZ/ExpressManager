using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Common;
using Common.Attributes;
using DataBase.Base.Interface;

namespace DataBase.Base.Model
{
    public class SysUserInfo : DbSetBase
    {
        public SysUserInfo()
        {
            Enable = true;
            Password = "123456".ToMD5();
        }

        [StringLength(30, MinimumLength = 4, ErrorMessage = "{0}长度为{2}-{1}个字符")]
        [Display(Name = "帐号")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "{0}长度少于{1}个字符")]
        [HiddenInput(DisplayValue = false)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "名称")]
        public string UserName { get; set; }

        [ScaffoldColumn(false)]
        public string RealName { get; set; }

        [Display(Name = "头像")]
        public string HeadIcon { get; set; }

        [Display(Name = "登陆时间")]
        [HiddenInput(DisplayValue = false)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastLoginTime { get; set; }

        [Display(Name = "是否可用")]
        public bool Enable { get; set; }

        [Display(Name = "角色")]
        [SelectList(typeof(ISysRoleInterface), "GetRole", true)]
        [DataFor(DataScoure = "SysRoleSysUserInfos", Value = "SysRoleId", Mode = UpdateMode.Replace)]
        public List<long> SysRoleIds { get; set; }

        public virtual ICollection<SysRoleSysUserInfo> SysRoleSysUserInfos { get; set; }

        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "新密码")]
        [NotMapped]
        public string PasswordNew { get; set; }

        [DataType(DataType.Password)]
        [StringLength(256, ErrorMessage = "{0}长度少于{1}个字符")]
        [Display(Name = "确认密码")]
        [NotMapped]
        public string PasswordNew2 { get; set; }


    }
}
