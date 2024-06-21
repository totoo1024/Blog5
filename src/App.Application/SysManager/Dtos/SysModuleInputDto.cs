using System.ComponentModel.DataAnnotations;

namespace App.Application.SysManager.Dtos
{
    public class SysModuleInputDto : EntityDto<string>
    {
        /// <summary>
        /// 上级ID（跟节点为0）
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required(ErrorMessage = "菜单名称为必填项")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "菜单名称限制2-20个字符")]
        public string FullName { get; set; }
        /// <summary>
        /// 菜单编码
        /// </summary>
        [Required(ErrorMessage = "菜单编码为必填项")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "菜单名称限制2-20个字符")]
        public string EnCode { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        public string UrlAddress { get; set; }
        /// <summary>
        /// 打开方式（null：框架页；_blank：新窗口打开）
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// 是否展开
        /// </summary>
        public bool IsExpand { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? SortCode { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(50, ErrorMessage = "描述限制50个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorAccountId { get; set; }
    }
}