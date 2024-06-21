namespace App.Application.Blog.Dtos
{
    public class ReplyInputDto
    {
        /// <summary>
        /// 评论信息ID
        /// </summary>
        public string RootId { get; set; }

        /// <summary>
        /// 上级评论ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 回复人ID
        /// </summary>
        public string FromId { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId  { get; set; }

        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleId { get; set; }
    }
}