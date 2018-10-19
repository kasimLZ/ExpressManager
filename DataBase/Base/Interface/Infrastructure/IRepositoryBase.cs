using DataBase.Base.Model;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataBase.Base.Infrastructure.Interface
{
    public interface IRepositoryBase<T> where T : class
    {
        void Add(T entity);
        int Commit();
        Task<int> CommitAsync();
        void Delete(long id);
        void Delete(T item);
        void Delete(Expression<Func<T, bool>> where);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(bool delete);
        IQueryable<T> GetAll(Expression<Func<T, bool>> where);
        T GetById(long id);
        void Remove(T item);
        void Remove(Expression<Func<T, bool>> where);
        void Save(long? id, T entity);
        void Update(T entity);
    }



}
