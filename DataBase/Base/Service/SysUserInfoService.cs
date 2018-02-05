using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq;

namespace DataBase.Base.Service
{
    public class SysUserInfoService : RepositoryBase<SysUserInfo>, SysUserInfoInterface
    {
        public SysUserInfoService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }

        public SysUserInfo GetUser(string UserName, string Password)
        {
            if(!string.IsNullOrEmpty(Password)) Password = Password.ToMD5();
            return base.GetAll(a => a.Login == UserName && a.Password == Password).FirstOrDefault();
        }
    }
}
