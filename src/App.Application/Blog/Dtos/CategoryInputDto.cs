namespace App.Application.Blog.Dtos
{
    public class CategoryInputDto : EntityDto<string>
    {
        /// <summary>
        /// 栏目名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 父级栏目ID
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}