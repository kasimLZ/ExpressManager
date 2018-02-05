using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;

namespace DataBase.Base.Interface
{
    public interface SysUserInfoInterface : IRepositoryBase<SysUserInfo>
    {

        SysUserInfo GetUser(string UserName, string Password);
    }
}
