using App.Core.Entities.Blog;
using App.Core.Repository;
using App.Framwork.Result;

namespace App.Application.Blog
{
    public class ArticleCategoryService : AppService<ArticleCategory>, IArticleCategoryService
    {
        public ArticleCategoryService(IAppRepository<ArticleCategory> repository) : base(repository)
        {
        }
    }
}