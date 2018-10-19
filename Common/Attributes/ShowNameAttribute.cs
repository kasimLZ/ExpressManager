using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ShowNameAttribute : Attribute
    {
        public uint sort = 0;

        public ShowNameAttribute() { }

        public ShowNameAttribute(uint sort) { this.sort = sort; }
    }
}
