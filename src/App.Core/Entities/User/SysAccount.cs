using System;
using App.Core.Data;

namespace App.Core.Entities.User
{
    /// <summary>
    /// 账户基本信息
    /// </summary>
    [Serializable]
    public class SysAccount : Entity<string>, ISoftDelete
    {
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
        /// 是否删除
        /// </summary>
        public bool DeleteMark { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool EnabledMark { get; set; } = true;
        /// <summary>
        /// 创建人账户ID
        /// </summary>
        public string CreatorAccountId { get; set; }
        /// <summary>
        /// 删除人
        /// </summary>
        public string DeleteAccountId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreatorTime { get; set; } = DateTime.Now;
    }
}
