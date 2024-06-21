using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public interface ICategoryService : IAppService<CategoryInfo>
    {
        /// <summary>
        /// 新增修改栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<UnifyResult> Save(CategoryInputDto dto);

        /// <summary>
        /// 获取所有一级分类
        /// </summary>
        /// <returns></returns>
        Task<List<CategoryDto>> GetRootCategories();
    }
}