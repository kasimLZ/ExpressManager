using DataBase.Base.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Base.Service.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        // Fields
        private readonly IApplicationDb _dataContext;

        // Methods
        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this._dataContext = databaseFactory.Get();
        }

        public int Commit()
        {
            return this._dataContext.Commit();
        }

        public Task<int> CommitAsync()
        {
            return this._dataContext.CommitAsync();
        }

    }
}
