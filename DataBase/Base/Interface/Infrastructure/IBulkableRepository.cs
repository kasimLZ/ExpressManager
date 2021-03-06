﻿using DataBase.Base.Model;
using System.Collections.Generic;

namespace DataBase.Base.Infrastructure.Interface
{
    public interface IBulkableRepository<T> : IRepositoryBase<T> where T : DbSetBase
    {
        void BulkInsert(IEnumerable<T> entities);
    }
}
