using FluentValidation;
using VaeMobility.Application.Catalog.DataTransfer.Request;

namespace VaeMobility.Application.Catalog.Validators;

public sealed class SaveCategoryRequestValidator : AbstractValidator<SaveCategoryRequest>
{
    public SaveCategoryRequestValidator()
    {
        RuleFor(request => request.Name).NotEmpty().MaximumLength(200);
        RuleFor(request => request.Slug).NotEmpty().MaximumLength(120);
        RuleFor(request => request.ShortDescription).NotEmpty().MaximumLength(500);
        RuleFor(request => request.Description).NotEmpty();
        RuleFor(request => request.Images).NotNull();
        RuleFor(request => request.Images.Explore).NotNull();
        RuleFor(request => request.Seo!.Title).MaximumLength(60).When(request => request.Seo?.Title is not null);
        RuleFor(request => request.Seo!.Description).MaximumLength(160).When(request => request.Seo?.Description is not null);
    }
}
