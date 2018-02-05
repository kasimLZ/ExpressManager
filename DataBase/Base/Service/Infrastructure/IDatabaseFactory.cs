using DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Base.Service.Infrastructure
{
    public interface IDatabaseFactory
    {
        // Methods
        IApplicationDb Get();
    }



}
