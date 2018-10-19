using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System;
using System.Collections;
using System.Web.Mvc;

namespace DataBase.Base.Interface
{
    public interface ISysRoleInterface : IRepositoryBase<SysRole>
    {
        MultiSelectList GetRole(IEnumerable list = null);
    }
}
