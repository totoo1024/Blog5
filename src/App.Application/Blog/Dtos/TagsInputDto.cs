namespace App.Application.Blog.Dtos
{
    public class TagsInputDto : EntityDto<string>
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 背景颜色
        /// </summary>
        public string BGColor { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public sbyte SortCode { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;
    }
}