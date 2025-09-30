using FluentValidation;

namespace ProductService.Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200);

            RuleFor(x => x.ManufactureEmail)
                .NotEmpty().EmailAddress();

            RuleFor(x => x.ManufacturePhone)
                .NotEmpty().MaximumLength(20);

            RuleFor(x => x.ProduceDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("ProduceDate cannot be in the future");
        }
    }
}
