using System;
using System.Linq;
using App.Framwork.DataValidation.Filters;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace App.Framwork.DataValidation.Extensions
{
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// 添加数据验证，支持数据注解
        /// </summary>
        /// <param name="mvcBuilder"></param>
        /// <param name="enableModelValidation">是否启用数据注解验证</param>
        public static void AddValidation(this IMvcBuilder mvcBuilder, bool enableModelValidation = true)
        {
            var types = Storage.Assemblys.SelectMany(x => x.GetTypes())
                .Where(x => typeof(IValidator).IsAssignableFrom(x) && !x.IsInterface && x.IsClass && !x.IsAbstract && !x.IsGenericType);
            mvcBuilder.AddFluentValidation(v =>
            {
                v.RunDefaultMvcValidationAfterFluentValidationExecutes = enableModelValidation;

            });
            var services = mvcBuilder.Services;
            foreach (var item in types)
            {
                Type dtoType = typeof(IValidator<>).MakeGenericType(item.BaseType.GenericTypeArguments[0]);

                services.TryAddTransient(dtoType, item);
            }

            mvcBuilder.AddMvcOptions(options =>
            {
                //禁止C# 8.0 验证非可空引用类型
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                //加入全局过滤器
                options.Filters.Add<DataValidationFilter>();
            });
        }
    }
}