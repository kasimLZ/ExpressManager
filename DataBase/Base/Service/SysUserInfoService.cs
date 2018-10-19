using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq;

namespace DataBase.Base.Service
{
    public class SysUserInfoService : RepositoryBase<SysUserInfo>, ISysUserInfoInterface
    {
        public SysUserInfoService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }

		public bool CheckRole(long UserId, string area, string controller, string action)
		{
			return GetAll(
				a => a.Id == UserId &&
					a.SysRoleSysUserInfos.Any(
						b => b.SysRole.SysRoleSysControllerSysActions.Any(
							c => c.SysControllerSysAction.SysController.SysArea.AreaName == area &&
								c.SysControllerSysAction.SysController.ControllerName == controller &&
								c.SysControllerSysAction.SysAction.ActionName == action
						)	
					)
			).Any();
		}

		public SysUserInfo GetUser(string UserName, string Password)
        {
            if(!string.IsNullOrEmpty(Password)) Password = Password.ToMD5();
            return base.GetAll(a => a.Login == UserName && a.Password == Password).FirstOrDefault();
        }
    }
}
