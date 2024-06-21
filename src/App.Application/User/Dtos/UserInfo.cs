namespace App.Application.User.Dtos
{
    /// <summary>
    /// QQ用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public int ret { get; set; }
        /// <summary>
        /// 错误信息（如果ret大于0，会有相应的错误信息提示）
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 出生年
        /// </summary>
        public string year { get; set; }
        /// <summary>
        /// 大小为30×30像素的QQ空间头像URL
        /// </summary>
        public string figureurl { get; set; }
        /// <summary>
        /// 大小为50×50像素的QQ空间头像URL
        /// </summary>
        public string figureurl_1 { get; set; }
        /// <summary>
        /// 大小为100×100像素的QQ空间头像URL
        /// </summary>
        public string figureurl_2 { get; set; }
        /// <summary>
        /// 大小为40×40像素的QQ头像URL
        /// </summary>
        public string figureurl_qq_1 { get; set; }
        /// <summary>
        /// 大小为100×100像素的QQ头像URL。需要注意，不是所有的用户都拥有QQ的100x100的头像，但40x40像素则是一定会有
        /// </summary>
        public string figureurl_qq_2 { get; set; }
    }
}