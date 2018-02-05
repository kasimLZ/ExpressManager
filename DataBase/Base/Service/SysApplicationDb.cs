using DataBase.DbLogger;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace DataBase.Base.Service
{
    public class SysApplicationDb : DbContext, IApplicationDb, IDisposable
    {
        // Methods
        public SysApplicationDb() : base("DefaultConnection")
        {
        }

        public SysApplicationDb(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }
        

        public virtual int Commit()
        {
            var sql = string.Empty;
            Database.Log = a => {
                sql += a;
            };
           // System.Data.Entity.Infrastructure.Interception.DbInterception.Add();
            return base.SaveChanges();
        }

        public virtual Task<int> CommitAsync()
        {
            return base.SaveChangesAsync();
        }

        DbEntityEntry IApplicationDb.Entry(object entity)
        {
            return base.Entry(entity);
        }

        DbEntityEntry<TEntity> IApplicationDb.Entry<TEntity>(TEntity entity) 
        {
            return base.Entry<TEntity>(entity);
        }

        IEnumerable<DbEntityValidationResult> IApplicationDb.GetValidationErrors()
        {
            return base.GetValidationErrors();
        }

        // Properties
        Database IApplicationDb.Database
        {
            get
            {
                return base.Database;
            }
        }
        
    }
}
