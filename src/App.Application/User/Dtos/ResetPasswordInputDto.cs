using System.ComponentModel.DataAnnotations;

namespace App.Application.User.Dtos
{
    public class ResetPasswordInputDto
    {
        /// <summary>
        /// 账户id
        /// </summary>
        [Required(ErrorMessage = "未知用户")]
        public string AccountId { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "新密码为必填项")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "密码长度限制6-18个字符")]
        [Compare(nameof(RePassword), ErrorMessage = "新密码与确认密码不一致")]
        public string Password { get; set; }

        /// <summary>
        /// 确认密码
        /// </summary>
        [Required(ErrorMessage = "确认密码为必填项")]
        public string RePassword { get; set; }
    }
}