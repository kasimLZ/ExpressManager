using System;
using System.Collections.Generic;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using DataBase.Base.Service.Infrastructure;
using System.Linq;
using Common;

namespace DataBase.Base.Service
{
    public class SysControllerService : RepositoryBase<SysController>, SysControllerInterface
    {
        public SysControllerService(IDatabaseFactory databaseFactory, ICurrentUser userInfo) 
            : base(databaseFactory, userInfo)
        {
        }

        public IEnumerable<SysController> GetBreadcrumbActions(string controller, string action)
        {
            var controllers = base.GetAll(a => a.ControllerName == controller).ToList();
            string code = string.Empty;
            if (controllers.Count() > 1)
            {
                var con = controllers.FirstOrDefault(a => a.ActionName == action);
                code = con == null ? controllers.FirstOrDefault().SystemId : con.SystemId;
            }
            else if (controllers.Count() == 1)
                code = controllers.First().SystemId;
            else
                return null;
            List<string> codes = new List<string>();
            for (int i = 0; i <= code.Length; i += 3)
                codes.Add(code.Substring(0, i));
            return base.GetAll(a => codes.Contains(a.SystemId)).OrderBy(a => a.SystemId).ToList().AsEnumerable();
        }
    }
}
