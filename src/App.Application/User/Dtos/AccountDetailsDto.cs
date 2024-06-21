using System;

namespace App.Application.User.Dtos
{
    public class AccountDetailsDto
    {
        /// <summary>
        /// 账户Id
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
        /// 登录密码
        /// </summary>
        public string Password { get; set; }
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
        /// <summary>
        /// 微信号
        /// </summary>
        public string WeChat { get; set; }
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatorAccountId { get; set; }
    }
}