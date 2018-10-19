using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;

namespace DataBase.Base.Interface
{
    public interface ISysUserInfoInterface : IRepositoryBase<SysUserInfo>
    {

        SysUserInfo GetUser(string UserName, string Password);

		bool CheckRole(long UserId, string area, string controller, string action);
    }
}
