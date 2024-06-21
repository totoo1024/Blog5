using App.Framwork.DependencyInjection;

namespace App.Core.Config
{
    /// <summary>
    /// 系统基础配置
    /// </summary>
    public class SysConfig : IConfig
    {
        /// <summary>
        /// 是否启用redis
        /// </summary>
        public bool UseRedis { get; set; }

        /// <summary>
        /// 是否启用极验行为验证
        /// </summary>
        public bool UseGeetest { get; set; }
    }
}