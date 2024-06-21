using App.Application.Blog.Dtos;
using FluentValidation;

namespace App.Application.Blog.Validators
{
    public class LeavemsgValidator : AbstractValidator<CommentInputDto>
    {
        public LeavemsgValidator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(500).WithMessage("留言内容限制1-500个字符");
        }
    }
}