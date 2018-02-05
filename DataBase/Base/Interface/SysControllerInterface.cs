using DataBase.Base.Infrastructure.Interface;
using DataBase.Base.Model;
using System.Collections.Generic;

namespace DataBase.Base.Interface
{
    public interface SysControllerInterface : IRepositoryBase<SysController>
    {
        IEnumerable<SysController> GetBreadcrumbActions(string controller, string action);
    }
}
