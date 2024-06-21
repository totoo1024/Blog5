using System.ComponentModel.DataAnnotations;

namespace App.Application.User.Dtos
{
    public class LoginInputDto
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "请输登录名")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "登录名限制2-20个字符")]
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required(ErrorMessage = "密码为必填项")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "密码长度限制6-18个字符")]
        public string Password { get; set; }
    }
}