using ECommerceAPI.Application.ViewModels.Products;
using FluentValidation;

namespace ECommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_CreateProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(n => n.Name)
                .NotEmpty()
                .NotNull()
                     .WithMessage("Please do not put empty Name")
                .MaximumLength(150)
                .MinimumLength(3)
                     .WithMessage("Name should be in between 3-150 characters");


            RuleFor(s => s.Stock)
                .NotNull()
                .NotEmpty()
                    .WithMessage("Please do not put empty Stock")
                .Must(s => s >= 0)
                    .WithMessage("Stock should be positive number");

            RuleFor(p => p.Price)
               .NotNull()
               .NotEmpty()
                   .WithMessage("Please do not put empty Price")
               .Must(p => p >= 0)
                   .WithMessage("Price should be positive number");
        }
    }
}
