using System;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq.Expressions;
using System.Linq;
using System.Collections;
using DataBase.Model;
using DataBase.Interface;
using Common;

namespace DataBase.Service
{
    public class NewsService : RepositoryBase<News>, NewsInterface
    {
        public NewsService(IDatabaseFactory databaseFactory, ICurrentUser userInfo)
            : base(databaseFactory, userInfo)
        {
        }

    }
}
