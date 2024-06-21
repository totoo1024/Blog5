namespace App.Application.User.Dtos
{
    /// <summary>
    /// QQ登录授权令牌信息
    /// </summary>
    public class QQAccessToken
    {
        /// <summary>
        /// 授权令牌，Access_Token
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// access token的有效期，单位为秒。（有效期3个月）
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 在授权自动续期步骤中，获取新的Access_Token时需要提供的参数。
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string error { get; set; }
        /// <summary>
        /// 错误消息
        /// </summary>
        public string error_description { get; set; }
    }
}