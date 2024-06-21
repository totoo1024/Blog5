using System;

namespace App.Framwork.DataValidation
{
    /// <summary>
    /// 忽略数据验证特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class IgnoreValidationAttribute : Attribute
    {

    }
}