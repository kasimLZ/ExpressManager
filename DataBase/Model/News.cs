using DataBase.Base.Model;
using DataBase.DbLogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Model
{
    [DbSetLogger(Level = LogLevel.All, SaveData = false)]
    public class News : DbSetBase
    {
        public string Title { get; set; }

        public string Content { get; set; }
    }
}
