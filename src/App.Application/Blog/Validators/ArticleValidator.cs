using System.Linq;
using App.Application.Blog.Dtos;
using FluentValidation;

namespace App.Application.Blog.Validators
{
    public class ArticleValidator : AbstractValidator<ArticleInputDto>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("文章标题为必填项")
                .Length(1, 50).WithMessage("文章标题不能超过50个字符");
            RuleFor(x => x.Content).NotEmpty().WithMessage("文章内容不能为空");
            RuleFor(x => x.Author).NotEmpty().Length(1, 20).WithMessage("作者限制1-20个字符");
            RuleFor(x => x.Categories).NotNull().Must(x => x.Any()).WithMessage("请选择栏目");
            RuleFor(x => x.Tags).NotNull().Must(x => x.Any()).WithMessage("请选择标签");
        }
    }
}