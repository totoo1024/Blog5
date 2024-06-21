using App.Framwork.DependencyInjection;
using App.Framwork.DependencyInjection.Attributes;

namespace App.Framwork.Generate.Geetest
{
    /// <summary>
    /// 极验基本配置
    /// </summary>
    [Section("Geetest")]
    public class GeetestConfig : IConfig
    {
        /// <summary>
        /// 极验行为验证公钥
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 极验行为验证私钥
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 极验行为验证创建地址
        /// </summary>
        public string RegisterUrl { get; set; }

        /// <summary>
        /// 极验行为验证地址
        /// </summary>
        public string ValidateUrl { get; set; }
    }
}