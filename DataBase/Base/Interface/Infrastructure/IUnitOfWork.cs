using System.Threading.Tasks;

namespace DataBase.Base.Infrastructure.Interface
{
    public interface IUnitOfWork
    {
        // Methods
        int Commit();
        Task<int> CommitAsync();
    }
}
