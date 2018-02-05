using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase.Base.Model
{
    public class SysErrorLog : DbSetBase
    {
        public SysErrorLog() { Method = HttpMethod.Unknow; }

        public string Code { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string FormParams { get; set; }

        public HttpMethod Method { get; set; }

        public string Url { get; set; }

        public string DetailMessage { get; set; }

        public string ErrorCode { get; set; }

        public long CustomUserId { get; set; }

        public string Borwser { get; set; }

        public string BorwserVersion { get; set; }

        public string System { get; set; }
    }
}
