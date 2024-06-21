namespace App.Application.Blog.Dtos
{
    public class CategoryDto : EntityDto<string>
    {
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级栏目ID
        /// </summary>
        public string ParentId { get; set; }
    }
}