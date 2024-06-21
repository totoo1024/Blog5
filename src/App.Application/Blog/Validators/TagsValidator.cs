using App.Application.Blog.Dtos;
using FluentValidation;

namespace App.Application.Blog.Validators
{
    public class TagsValidator : AbstractValidator<TagsInputDto>
    {
        public TagsValidator()
        {
            RuleFor(x => x.TagName).NotEmpty().MaximumLength(20).WithMessage("标签长度仅限1-20个字符");
            RuleFor(x => x.BGColor).NotEmpty().WithMessage("背景颜色为必填项");
        }
    }
}