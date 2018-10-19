using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false, Inherited = true)]
    public class OrderAttribute : Attribute
    {
        private static readonly string[] code = { "asc", "desc", "ascending", "descending" };

        private string _order { get; set; }

        public string Field { get; set; }

        public string Order
        {
            get
            {
                return string.IsNullOrEmpty(_order) ? code.First() : _order;
            }
            set
            {
                _order = code.FirstOrDefault(a => value.Equals(a, StringComparison.CurrentCultureIgnoreCase));
                if (string.IsNullOrEmpty(_order)) _order = code.First();
            }
        }

    }
}
