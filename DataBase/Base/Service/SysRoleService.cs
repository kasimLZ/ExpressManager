using System;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq.Expressions;
using System.Linq;
using System.Collections;
using Common;

namespace DataBase.Base.Service
{
    public class SysRoleService : RepositoryBase<SysRole>, SysRoleInterface
    {
        public SysRoleService(IDatabaseFactory databaseFactory, ICurrentUser userInfo)
            : base(databaseFactory, userInfo)
        {
        }
    }
}
