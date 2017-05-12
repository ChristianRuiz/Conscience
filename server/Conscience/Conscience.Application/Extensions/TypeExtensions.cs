using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Conscience.Application.Graph
{
    public static class TypeExtensions
    {
        public static bool HasProperty(this Type t, string propertyPath)
        {
            if (propertyPath.Contains("."))
            {
                var directPropertyPath = propertyPath.Split('.').First();
                var subPath = propertyPath.Substring(directPropertyPath.Length + 1);
                var property = t.GetProperty(directPropertyPath, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (property == null)
                    return false;

                return property.PropertyType.HasProperty(subPath);
            }

            return t.GetProperty(propertyPath, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) != null;
        }
    }
}
