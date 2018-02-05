using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SearchAttribute : Attribute
    {
        public SearchAttribute()
        {
            
        }

        public SearchAttribute(string Field)
        {
            this.Field = Field;
        }

        public string Field { get; }

        
    }
}
