namespace App.Application.User.Dtos
{
    public class OpenIdInfo
    {
        /// <summary>
        /// 对应APPID
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// 网站上唯一对应用户身份的标识
        /// </summary>
        public string openid { get; set; }
    }
}