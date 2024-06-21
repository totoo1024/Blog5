using System.ComponentModel.DataAnnotations;

namespace App.Application.SysManager.Dtos
{
    public class SysButtonInputDto : EntityDto<string>
    {
        /// <summary>
        /// 模块Id（对应SysModule表主键）
        /// </summary>
        [Required(ErrorMessage = "请选择菜单")]
        public string SysModuleId { get; set; }
        /// <summary>
        /// 按钮名称
        /// </summary>
        [Required(ErrorMessage = "按钮名称是必填项")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "按钮名称限制2-10个字符")]
        public string FullName { get; set; }
        /// <summary>
        /// 按钮图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 按钮编码
        /// </summary>
        [Required(ErrorMessage = "按钮编码是必填项")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "按钮编码限制2-20个字符")]
        public string EnCode { get; set; }
        /// <summary>
        /// 按钮位置（1：工具栏；2：表格栏；3：数据列）
        /// </summary>
        public int? Location { get; set; }
        /// <summary>
        /// JS事件
        /// </summary>
        public string JsEvent { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [MaxLength(120, ErrorMessage = "链接地址限制120个字符")]
        public string UrlAddress { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int? SortCode { get; set; }
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