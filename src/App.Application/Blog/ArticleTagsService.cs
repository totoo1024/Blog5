using App.Core.Entities.Blog;
using App.Core.Repository;

namespace App.Application.Blog
{
    public class ArticleTagsService : AppService<ArticleTags>, IArticleTagsService
    {
        public ArticleTagsService(IAppRepository<ArticleTags> repository) : base(repository)
        {
        }
    }
}