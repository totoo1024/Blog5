using System;
using System.ComponentModel.DataAnnotations;

namespace App.Application.Blog.Dtos
{
    public class TimeLineInputDto : EntityDto<string>
    {
        /// <summary>
        /// 内容
        /// </summary>
        [Required(ErrorMessage = "内容为必填项")]
        [StringLength(maximumLength: 100, MinimumLength = 1, ErrorMessage = "内容限制1-100个字符")]
        public string Content { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime PublishDate { get; set; }
    }
}