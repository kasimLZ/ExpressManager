using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;

namespace DataBase.Base.Service
{
    public class SysErrorLogService : RepositoryBase<SysErrorLog>, SysErrorLogInterface
    {
        public SysErrorLogService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }
    }
}
