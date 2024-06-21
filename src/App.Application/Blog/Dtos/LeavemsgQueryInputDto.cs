namespace App.Application.Blog.Dtos
{
    public class LeavemsgQueryInputDto : PageInputDto
    {
        /// <summary>
        /// 评论ID
        /// </summary>
        public string RootId { get; set; }


        /// <summary>
        /// 文章ID
        /// </summary>
        public string Aid { get; set; }

        /// <summary>
        /// 评论下显示的回复数量
        /// </summary>
        public int ChildSize { get; set; } = 3;
    }
}