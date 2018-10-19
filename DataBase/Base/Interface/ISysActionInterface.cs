using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Collections;

namespace DataBase.Base.Interface
{
    public interface ISysActionInterface : IRepositoryBase<SysAction>
    {
        MultiSelectList SelectA(IEnumerable list = null);
    }
}
