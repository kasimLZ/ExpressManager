using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System;
using System.Web.Mvc;

namespace DataBase.Base.Interface
{
    public interface IUserDictTypeInterface : IRepositoryBase<UserDictType>
    {
        SelectList GetUserDictType(long id);
    }
}
