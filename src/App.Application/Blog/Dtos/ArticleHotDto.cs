using System;

namespace App.Application.Blog.Dtos
{
    /// <summary>
    /// 热门文章
    /// </summary>
    public class ArticleHotDto : EntityDto<string>
    {
        /// <summary>
        /// 阅读次数
        /// </summary>
        public int ReadTimes { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
}