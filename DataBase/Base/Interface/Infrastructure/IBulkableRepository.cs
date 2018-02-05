using System.Collections.Generic;

namespace DataBase.Base.Infrastructure.Interface
{
    public interface IBulkableRepository<T> : IRepositoryBase<T> where T : class
    {
        void BulkInsert(IEnumerable<T> entities);
    }
}
