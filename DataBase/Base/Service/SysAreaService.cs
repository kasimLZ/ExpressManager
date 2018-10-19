using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;

namespace DataBase.Base.Service
{
    public class SysAreaService : RepositoryBase<SysArea>, ISysAreaInterface
    {
        public SysAreaService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }
    }
}
