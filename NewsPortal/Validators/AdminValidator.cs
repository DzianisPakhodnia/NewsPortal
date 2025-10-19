using FluentValidation;
using NewsPortal.Models;

namespace NewsPortal.Validators
{
    public class AdminValidator : AbstractValidator<Admin>
    {
        public AdminValidator() 
        {
           RuleFor(a =>  a.Name)
                .NotEmpty().WithMessage("Имя обязательно");

            RuleFor(a => a.Email)
                .NotEmpty().WithMessage("Email обязателен")
                .EmailAddress().WithMessage("Некорректный формат email");

            RuleFor(a => a.PasswordHash)
                .NotEmpty().WithMessage("Пароль обязателен")
                .MinimumLength(6).WithMessage("Пароль должен содержать минимум 6 символов");

        }
    }
}
