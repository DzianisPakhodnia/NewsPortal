using FluentValidation;
using NewsPortal.Models;

namespace NewsPortal.Validator
{
    public class NewsValidator : AbstractValidator<News>
    {
        public NewsValidator() 
        {
            RuleFor(x => x.Title)
                 .NotEmpty().WithMessage("Заголовок обязателен")
                 .MaximumLength(150).WithMessage("Заголовок не может превышать 150 символов");

            RuleFor(x => x.Subtitle)
                .MaximumLength(200).WithMessage("Подзаголовок не может превышать 200 символов");

            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Текст новости обязателен")
                .MinimumLength(20).WithMessage("Текст должен содержать минимум 20 символов");

            RuleFor(x => x.ImageFile)
                .Must(file => file == null || file.Length < 5 * 1024 * 1024)
                .WithMessage("Размер изображения не должен превышать 5 МБ");
               
        }
    }
}
