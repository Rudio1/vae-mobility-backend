using FluentValidation;
using VaeMobility.Application.Catalog.DataTransfer.Request;
using VaeMobility.Domain.Catalog.Enums;

namespace VaeMobility.Application.Catalog.Validators;

public sealed class SaveProductRequestValidator : AbstractValidator<SaveProductRequest>
{
    public SaveProductRequestValidator()
    {
        RuleFor(request => request.CategoryId).NotEmpty();
        RuleFor(request => request.Name).NotEmpty().MaximumLength(200);
        RuleFor(request => request.Slug).NotEmpty().MaximumLength(120);
        RuleFor(request => request.Status).Must(status => ProductStatuses.All.Contains(status));
        RuleFor(request => request.Summary).NotEmpty().MaximumLength(160);
        RuleFor(request => request.Description).NotEmpty();
        RuleFor(request => request.Images).NotNull();
        RuleFor(request => request.Highlights).NotNull();
        RuleFor(request => request.Specs).NotNull();
        RuleFor(request => request.Variants).NotNull();
        RuleFor(request => request.Price!.Amount).GreaterThan(0).When(request => request.Price is not null);
        RuleFor(request => request.Seo!.Title).MaximumLength(60).When(request => request.Seo?.Title is not null);
        RuleFor(request => request.Seo!.Description).MaximumLength(160).When(request => request.Seo?.Description is not null);
    }
}
