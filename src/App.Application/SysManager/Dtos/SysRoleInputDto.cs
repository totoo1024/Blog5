using System.ComponentModel.DataAnnotations;

namespace App.Application.SysManager.Dtos
{
    public class SysRoleInputDto : EntityDto<string>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage = "角色名称是必填项")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "角色名称限制2-20个字符")]
        public string FullName { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        [Required(ErrorMessage = "角色编码是必填项")]
        public string EnCode { get; set; }
        /// <summary>
        /// 排序码
        /// </summary>
        public int? SortCode { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(100, ErrorMessage = "描述限制100个字符")]
        public string Description { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorAccountId { get; set; }
    }
}