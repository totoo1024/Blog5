using System;
namespace App.Hosting
{
    /// <summary>
    /// 仅标记controller，忽略权限验证
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAccessAttribute : Attribute
    {

    }
}