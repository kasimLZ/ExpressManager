using Common;
using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataBase.Base.Service.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        // Fields
        private readonly IDatabaseFactory _databaseFactory;
        private readonly IApplicationDb _dataContext;
        private readonly IDbSet<T> _dbset;
        public readonly ICurrentUser _userInfo;

        protected RepositoryBase(IDatabaseFactory databaseFactory, ICurrentUser userInfo)
        {
            this._databaseFactory = databaseFactory;
            this._dataContext = databaseFactory.Get();
            this._dbset = this._dataContext.Set<T>();
            this._userInfo = userInfo;
        }

        public virtual void Add(T entity)
        {
            IDbSetBase base2 = entity as IDbSetBase;
            if (base2 == null)
            {
                this._dbset.Add(entity);
                return;
            }
            base2.CreatedDate = DateTime.Now;
            this._dbset.Add(base2 as T);
        }
        

        public int Commit()
        {
            return this._dataContext.Commit();
        }

        public Task<int> CommitAsync()
        {
            return this._dataContext.CommitAsync();
        }

        public virtual void Delete(long id)
        {
            T byId = this.GetById(id);
            this.Delete(byId);
        }

        public virtual void Delete(T item)
        {
            IDbSetBase base2 = item as IDbSetBase;
            if (base2 != null)
            {
                base2.Deleted = true;
            }
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            foreach (T local in this.GetAll(where))
            {
                this.Delete(local);
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.GetAll(true);
        }

        public virtual IQueryable<T> GetAll(bool delete)
        {
            var field = typeof(T).GetProperties().FirstOrDefault(a => a.Name.ToLower().Equals("deleted"));
            Expression<Func<T, bool>> express;
            if (field != null && field.GetType() == typeof(bool))
            {
                express = Expression.Lambda<Func<T, bool>>(Expression.Equal(Expression.Property(Expression.Parameter(typeof(T), "a"), "Delete"), Expression.Constant(delete)));
            }
            else
            {
                express = a => true;
            }
            return this._dbset.Where(express);
        }

        public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> where)
        {
            return this.GetAll().Where(where);
        }

        public virtual T GetById(long id)
        {
            object[] keyValues = new object[] { id };
            return this._dbset.Find(keyValues);
        }

        private bool IsSimpleProperty(string propertyName, DbEntityEntry entry)
        {
            return (entry.Member(propertyName) is DbPropertyEntry);
        }

        public virtual void Remove(T item)
        {
            this._dbset.Remove(item);
        }

        public virtual void Remove(Expression<Func<T, bool>> where)
        {
            foreach (T local in this.GetAll(where))
            {
                this.Remove(local);
            }
        }

        public virtual void Save(long? id, T entity)
        {
            IDbSetBase base2 = entity as IDbSetBase;
            if (base2 == null)
            {
                if (id.HasValue)
                {
                    this.Update(entity);
                }
                else
                {
                    this.Add(entity);
                }
                return;
            }
            if (!id.HasValue)
            {
                base2.Id = SFID.NewID();
                this.Add(base2 as T);
                return;
            }
            base2.CreatedDate = DateTime.Now;
            this.Update(base2 as T);
        }


        public virtual void Update(T entity)
        {
            this._dbset.Attach(entity);
            this._dataContext.Entry<T>(entity).State = EntityState.Modified;
            (entity as IDbSetBase).UpdatedDate = DateTime.Now;
            if (this._dataContext.Entry<T>(entity).Property("CreatedDate") != null)
            {
                this._dataContext.Entry<T>(entity).Property("CreatedDate").IsModified = false;
            }

        }

    }
}
