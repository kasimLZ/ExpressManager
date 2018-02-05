using Common;
using DataBase.Base.Interface;
using DataBase.Base.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Web.Helper
{
    public class CurrentUser : ICurrentUser
    {
        private readonly NameValueCollection UserInfo = new NameValueCollection();

        public CurrentUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(HttpContext.Current.User.Identity.Name);
                foreach (var item in dict) { UserInfo[item.Key] = item.Value; }
            }
        }

        public int? Id
        {
            get
            {
                int id = 0;
                if (int.TryParse(UserInfo["UserId"], out id))
                    return id;
                else
                    return null;
            }
        }

        public string Name {
            get {
                return UserInfo["UserName"];
            }
        }
        

        public NameValueCollection Cookie {

            get {
                return UserInfo;
            }
        }

       
    }
}