using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace App.Framwork.ValueType
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 缓存
        /// </summary>
        private static ConcurrentDictionary<string, string> enumDic = new ConcurrentDictionary<string, string>();
        public static string Description(this Enum value)
        {
            Type type = value.GetType();
            string vs = value.ToString(), key = $"{type.FullName}_{vs}";
            if (enumDic.ContainsKey(key))
            {
                return enumDic[key];
            }
            FieldInfo fieldInfo = type.GetField(vs);
            DescriptionAttribute descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            if (descriptionAttribute != null)
            {
                string desc = descriptionAttribute.Description;
                enumDic.TryAdd(key, desc);
                return desc;
            }
            return string.Empty;
        }
    }
}