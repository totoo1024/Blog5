using System.Collections.Generic;

namespace App.Application.SysManager.Dtos
{
    public class MenuSettingDto
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 链接
        /// </summary>
        public string href { get; set; }

        /// <summary>
        /// 是否展开
        /// </summary>
        public bool spread { get; set; } = false;

        /// <summary>
        /// 子菜单
        /// </summary>
        public List<MenuSettingDto> children { get; set; }
    }
}