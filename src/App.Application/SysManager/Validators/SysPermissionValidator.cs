using System.Linq;
using App.Application.SysManager.Dtos;
using FluentValidation;

namespace App.Application.SysManager.Validators
{
    public class SysPermissionValidator : AbstractValidator<SysPermissionInputDto>
    {
        public SysPermissionValidator()
        {
            RuleFor(x => x.RoleId).NotEmpty().WithMessage("所分配角色不存在");
            RuleFor(x => x.Permission).Must(x => x.Any()).WithMessage("分配的权限不能为空");
        }
    }
}