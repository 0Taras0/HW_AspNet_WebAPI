using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Domain.Data;
using Core.Model.Category;

namespace Core.Validators.Category
{
    public class CategoryUpdateValidator : AbstractValidator<CategoryUpdateModel>
    {
        public CategoryUpdateValidator(AppDbContext db)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Назва є обов'язковою")
                .MaximumLength(250)
                .WithMessage("Назва повинна містити не більше 250 символів")
                .Must(name => !string.IsNullOrEmpty(name))
                .WithMessage("Назва не може бути порожньою або null")
                .MustAsync(async (model, name, cancellation) =>
                    !await db.Categories
                        .AnyAsync(c => c.Name.ToLower() == name.ToLower().Trim() && c.Id != model.Id, cancellation))
                .WithMessage("Категорія з такою назвою вже існує");

            RuleFor(x => x.Slug)
                .NotEmpty()
                .WithMessage("Слаг є обов'язковим")
                .MaximumLength(250)
                .WithMessage("Слаг повинен містити не більше 250 символів");
        }
    }
}
