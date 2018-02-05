using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Areas.Account.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string ValidateCode { get; set; }

        public bool RememberMe { get; set; }

        public string ErrorMsg { get; set; }
    }
}