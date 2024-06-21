namespace App.Application.Blog.Dtos
{
    public class CommentInputDto
    {
        /// <summary>
        /// 文章ID
        /// </summary>
        public string ArticleId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论人ID
        /// </summary>
        public string UserId { get; set; }
    }
}