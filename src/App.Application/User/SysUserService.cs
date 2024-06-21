using App.Core.Entities.User;
using App.Core.Repository;

namespace App.Application.User
{
    public class SysUserService : AppService<SysUser>, ISysUserService
    {
        public SysUserService(IAppRepository<SysUser> repository) : base(repository)
        {
        }


    }
}