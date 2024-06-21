using System;
using System.Collections.Generic;

namespace App.Application.Blog.Dtos
{
    /// <summary>
    /// 文章信息Dto
    /// </summary>
    public class ArticleInputDto
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 创作类型：0-原创；1-转载
        /// </summary>
        public sbyte? CreativeType { get; set; }

        /// <summary>
        /// 文章来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 来源链接
        /// </summary>
        public string SourceLink { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 内容摘要
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否置顶
        /// </summary>
        public bool IsTop { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 文章标签
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 栏目
        /// </summary>
        public List<string> Categories { get; set; }
    }
}