namespace App.Application.Blog.Dtos
{
    public class NoticeInputDto : EntityDto<string>
    {
        /// <summary>
        /// 通知内容
        /// </summary>
        public string Description { get; set; }

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