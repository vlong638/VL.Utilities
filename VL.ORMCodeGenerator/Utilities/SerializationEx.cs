using System;
using System.Collections.Generic;
using System.Linq;

namespace VL.ORMCodeGenerator.Utilities
{
    public static class SerializationEx
    {
        public const char StringContentSplitter = ',';
        public const char StringValueSplitter = '|';
        public static string SerializeClassToString<T>(this T t)
        {
            var classType = typeof(T);
            List<string> subContent = new List<string>();
            var properties = classType.GetProperties();
            foreach (var property in properties)
            {
                subContent.Add(property.Name + StringValueSplitter + property.GetValue(t));
            }
            return string.Join(StringContentSplitter.ToString(), subContent);
        }

        public static T SerializeClassFromString<T>(this string contentString) where T : new()
        {
            T t = new T();
            var valuePairs = contentString.Split(StringContentSplitter);
            var properties = typeof(T).GetProperties();
            foreach (var valuePair in valuePairs)
            {
                var values = valuePair.Split(StringValueSplitter);
                var property = properties.FirstOrDefault(c => c.Name == values[0]);
                if (property != null)
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(t, values[1]);
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        property.SetValue(t, Convert.ToBoolean(values[1]));
                    }
                    else if (property.PropertyType.BaseType == typeof(Enum))
                    {
                        property.SetValue(t, Enum.Parse(property.PropertyType, values[1]));
                    }
                    else
                    {
                        throw new NotImplementedException("未支持该类型的属性赋值");
                    }
                }
            }
            return t;
        }
    }
}
