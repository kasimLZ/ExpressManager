using System;
using System.Collections.Generic;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq;
using Common;

namespace DataBase.Base.Service
{
    public class SysActionService : RepositoryBase<SysAction>, SysActionInterface
    {
        public SysActionService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }
        
    }
}
