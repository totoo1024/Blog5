using System.ComponentModel;

namespace App.Core.Share
{
    /// <summary>
    /// 文章创作类型
    /// </summary>
    public enum CreativeType
    {
        /// <summary>
        /// 原创
        /// </summary>
        [Description("原创")]
        Original,

        /// <summary>
        /// 转载
        /// </summary>
        [Description("转载")]
        Reprint
    }
}