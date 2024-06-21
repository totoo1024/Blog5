using System;

namespace App.Framwork.DependencyInjection.Attributes
{
    /// <summary>
    /// appsetting配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SectionAttribute : Attribute
    {
        /// <summary>
        /// appsettings节点
        /// </summary>
        public string Section { get; }
        public SectionAttribute(string section)
        {
            Section = section;
        }
    }
}