using App.Framwork.DependencyInjection;

namespace App.Core.Config
{
    /// <summary>
    /// QQ授权登录配置
    /// </summary>
    public class QQAuthConfig : IConfig
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用key
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string CallbackUrl { get; set; }
    }
}