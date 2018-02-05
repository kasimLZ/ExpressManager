using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;

namespace DataBase.Base.Service
{
    public class SysRoleSysUserInfoService : RepositoryBase<SysRoleSysUserInfo>, SysRoleSysUserInfoInterface
    {
        public SysRoleSysUserInfoService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }
    }
}
