namespace App.Application.Blog.Dtos
{
    public class ArticleQueryInputDto : PageInputDto
    {
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords { get; set; }

        /// <summary>
        /// 栏目ID或者标签ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 1：栏目id；2：标签id
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }
    }
}