using App.Application.Blog.Dtos;
using FluentValidation;

namespace App.Application.Blog.Validators
{
    public class ReplyValidator : AbstractValidator<ReplyInputDto>
    {
        public ReplyValidator()
        {
            RuleFor(x => x.Content).NotEmpty().MaximumLength(500).WithMessage("回复内容限制1-500个字符");

            RuleFor(x => x.RootId).NotEmpty().WithMessage("缺少参数");

            RuleFor(x => x.FromId).NotEmpty().WithMessage("缺少参数");

            RuleFor(x => x.FromId).NotEqual(x => x.UserId).WithMessage("不能回复自己的留言");


        }
    }
}