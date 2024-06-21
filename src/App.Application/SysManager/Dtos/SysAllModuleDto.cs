using System.Collections.Generic;
using App.Core.Entities.SysManager;

namespace App.Application.SysManager.Dtos
{
    public class SysAllModuleDto
    {
        public List<MenuSettingDto> topMenus { get; set; }

        public Dictionary<string, List<MenuSettingDto>> childMenus { get; set; }

        public Dictionary<string, List<SysButton>> rowButtons { get; set; }

        public Dictionary<string, List<SysButton>> toolButtons { get; set; }
    }
}