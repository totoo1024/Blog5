using System.Collections.Generic;

namespace App.Application.SysManager.Dtos
{
    public class SysPermissionInputDto
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 分配权限菜单按钮id集合
        /// </summary>
        public List<string> Permission { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserId { get; set; }
    }
}