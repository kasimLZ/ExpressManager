using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Linq
{
    public static class TypeExtensions
    {
        public static string IdentifierPropertyName(this Type type)
        {
            var keyField = type.GetProperties().Where(info => info.AttributeExists<KeyAttribute>());
            if (keyField.Count() == 1)
            {
                return keyField.First().Name;
            }
            else if (!keyField.Any())
            {
                keyField = type.GetProperties();
            }

            if (keyField.Any(p => p.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase)))
            {
                return keyField.First(p => p.Name.Equals("id", StringComparison.CurrentCultureIgnoreCase)).Name;
            }
            else if (keyField.Any(p => p.Name.Equals(type.Name + "id", StringComparison.CurrentCultureIgnoreCase)))
            {
                return keyField.First(p => p.Name.Equals(type.Name + "id", StringComparison.CurrentCultureIgnoreCase)).Name;
            }

            return keyField.First().Name;
        }

        public static bool AttributeExists<T>(this PropertyInfo propertyInfo) where T : class
        {
            var attribute = propertyInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
            return attribute != null;
        }
    }
}
