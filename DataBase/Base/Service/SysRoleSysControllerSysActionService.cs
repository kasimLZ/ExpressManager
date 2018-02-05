using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;

namespace DataBase.Base.Service
{
    public class SysRoleSysControllerSysActionService : RepositoryBase<SysRoleSysControllerSysAction>, SysRoleSysControllerSysActionInterface
    {
        public SysRoleSysControllerSysActionService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }
    }
}
