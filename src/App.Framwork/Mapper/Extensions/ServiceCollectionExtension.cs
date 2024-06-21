using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace App.Framwork.Mapper.Extensions
{
    /// <summary>
    /// 使用mapper自动映射
    /// </summary>
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            TypeAdapterConfig.GlobalSettings.Scan(Storage.Assemblys.ToArray());
            TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible).PreserveReference(true);
            return services;
        }
    }
}