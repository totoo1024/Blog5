using System;

namespace App.Application.User.Dtos
{
    public class AuthUser
    {
        /// <summary>
        /// 登录ID
        /// </summary>
        public string LoginId { get; set; }

        /// <summary>
        /// 账户ID
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 用户ID 
        /// </summary>
        public string SysUserId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 所属部门
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 所属角色
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadIcon { get; set; }
        /// <summary>
        /// 性别(true：男；false：女)
        /// </summary>
        public bool Gender { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string MobilePhone { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }
    }
}