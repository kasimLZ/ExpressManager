using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace DataBase.Base.Service
{
    public class UserDictTypeService : RepositoryBase<UserDictType>, IUserDictTypeInterface
    {
        public UserDictTypeService(IDatabaseFactory databaseFactory, ICurrentUser userInfo)
            : base(databaseFactory, userInfo)
        {
        }

        public SelectList GetUserDictType(long id)
        {
            return new SelectList(GetAll(a => !a.Deleted), "Id", "DictTypeName", id);
        }
    }
}
