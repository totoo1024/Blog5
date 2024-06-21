using App.Application.Blog.Dtos;
using FluentValidation;

namespace App.Application.Blog.Validators
{
    public class NoticeValidator : AbstractValidator<NoticeInputDto>
    {
        public NoticeValidator()
        {
            RuleFor(x => x.Description).NotEmpty().MaximumLength(100).WithMessage("通知内容限制1-100个字符");

            RuleFor(x => x.Target).NotEmpty().WithMessage("跳转方式不能为空");
        }
    }
}