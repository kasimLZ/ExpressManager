using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class LookupAttribute : DataTypeAttribute
    {
        private RouteValueDictionary _routeValueDict = null;
        private string _routeString = null;

        public LookupAttribute() : base("Lookup") { }

        public string Url { get; set; }
        
        public string RouteName { get; set; }
        public string RouteString
        {
            set
            {
                _routeValueDict = new RouteValueDictionary();
                foreach (var p in value.Split(','))
                {
                    var arr = p.Split('=');
                    if (arr.Length == 2)
                        _routeValueDict[arr[0]] = arr[1];
                }
                _routeString = value;
            }
            get { return _routeString; }
        }
        
        public RouteValueDictionary RouteValues { get { return _routeValueDict; } }

        public bool Multi { get; set; }

        public string ShowName { get; set; }

        public string Condition { get; set; }
    }
}
