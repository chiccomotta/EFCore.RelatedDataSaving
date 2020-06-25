using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace EFRelatedData
{
    public static class PropertiesCopier
    {
        public static void CopyPropertiesFrom(this object self, object parent)
        {
            var fromProperties = parent.GetType().GetProperties();
            var toProperties = self.GetType().GetProperties();

            foreach (var fromProperty in fromProperties)
            {
                foreach (var toProperty in toProperties)
                {
                    if (fromProperty.Name == toProperty.Name && fromProperty.PropertyType == toProperty.PropertyType)
                    {
                        toProperty.SetValue(self, fromProperty.GetValue(parent));
                        break;
                    }
                }
            }
        }

        public static T CopyPropertiesFrom2<T>(T self, Dictionary<string, dynamic> parent)
        {
            var toProperties = self.GetType().GetProperties();

            foreach (var key in parent.Keys)
            {
                foreach (var toProperty in toProperties)
                {
                    if (key == toProperty.Name)
                    {
                        toProperty.SetValue(self, ChangeType(parent[key], toProperty.PropertyType));
                        break;
                    }
                }
            }

            return self;
        }

        public static object ChangeType(object value, Type conversion) 
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>)) 
            {
                if (value == null) 
                { 
                    return null; 
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t);
        }
    }

    public class BlogDto
    {
        public string Url { get; set; }
        public int? Ranking { get; set; }
    }
}