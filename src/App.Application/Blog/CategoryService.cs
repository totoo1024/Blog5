using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Application.Blog.Dtos;
using App.Core.Entities.Blog;
using App.Core.Repository;
using App.Framwork.Generate;
using App.Framwork.Result;
using Mapster;

namespace App.Application.Blog
{
    public class CategoryService : AppService<CategoryInfo>, ICategoryService
    {
        public CategoryService(IAppRepository<CategoryInfo> repository) : base(repository)
        {
        }

        /// <summary>
        /// 新增修改栏目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<UnifyResult> Save(CategoryInputDto dto)
        {
            bool isAny = await Repository.AnyAsync(c => c.CategoryName == dto.CategoryName && c.Id != dto.Id);
            if (isAny)
            {
                return "文章栏目已存在";
            }

            var category = dto.Adapt<CategoryInfo>();
            if (string.IsNullOrWhiteSpace(category.Id))
            {
                category.Id = SnowflakeId.NextStringId();
                return await InsertRemoveCacheAsync(category);
            }
            else
            {
                if (category.Id == category.ParentId)
                {
                    return "上级栏目不能与当前栏目相同";
                }
                if (category.ParentId != "0")
                {
                    var list = GetList(m => m.DeleteMark == false && m.EnabledMark);
                    if (GetChildCategory(list, category.Id).Any(o => o.Id == category.ParentId))
                    {
                        return "无法将一级菜单指定到子级栏目下";
                    }
                }
                return await UpdateRemoveCacheAsync(category, i => new { i.CreatorTime, i.DeleteMark });
            }
        }

        /// <summary>
        /// 获取所有一级分类
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDto>> GetRootCategories()
        {
            return await GetListAsync<CategoryDto>(x => x.ParentId == "0");
        }

        /// <summary>
        /// 获取指定分类下所有子分类
        /// </summary>
        /// <param name="list">所有分类</param>
        /// <param name="id">指定分类ID</param>
        /// <returns></returns>
        private List<CategoryInfo> GetChildCategory(List<CategoryInfo> list, string id)
        {
            List<CategoryInfo> modules = new List<CategoryInfo>();
            foreach (var item in list.Where(m => m.ParentId == id))
            {
                modules.Add(item);
                GetChildCategory(list, item.Id);
            }
            return modules;
        }
    }
}