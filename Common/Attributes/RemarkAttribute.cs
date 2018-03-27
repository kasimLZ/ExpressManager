using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Class,AllowMultiple = false, Inherited = false)]
    public class RemarkAttribute : Attribute
    {
        public RemarkAttribute(string Context)
        {
            this.Context = Context;
        }

        public string Context { get; set; }
    }
}
