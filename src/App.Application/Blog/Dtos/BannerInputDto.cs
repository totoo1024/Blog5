namespace App.Application.Blog.Dtos
{
    public class BannerInputDto : EntityDto<string>
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// 跳转方式
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public sbyte? SortCode { get; set; }
    }
}