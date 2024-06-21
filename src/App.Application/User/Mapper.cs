using App.Application.User.Dtos;
using App.Core.Entities.User;
using Mapster;

namespace App.Application.User
{
    public class Mapper : IMapperTag
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<AccountDetailsDto, SysAccount>()
                .Map(s => s.Id, d => d.AccountId);

            config.ForType<AccountDetailsDto, SysUser>()
                .Map(d => d.Id, s => s.SysUserId);
        }
    }
}