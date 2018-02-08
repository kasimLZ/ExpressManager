using System;
using System.Collections.Generic;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq;
using Common;
using System.Web.Mvc;
using System.Collections;

namespace DataBase.Base.Service
{
    public class SysActionService : RepositoryBase<SysAction>, SysActionInterface
    {
        public SysActionService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }

        public MultiSelectList SelectA(IEnumerable list = null)
        {
            return new MultiSelectList(GetAll(), "Id", "ActionDisplayName", list);
        }
    }
}
