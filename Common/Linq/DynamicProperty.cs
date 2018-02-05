using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Linq
{
    public class DynamicProperty
    {
        private string name;
        private Type type;
        
        public DynamicProperty(string name, Type type)
        {
            this.name = name ?? throw new ArgumentNullException("name");
            this.type = type ?? throw new ArgumentNullException("type");
        }
        
        public string Name
        {
            get
            {
                return name;
            }
        }

        public Type Type
        {
            get
            {
                return type;
            }
        }
    }



}
