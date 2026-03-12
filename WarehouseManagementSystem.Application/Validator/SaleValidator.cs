using FluentValidation;
using WarehouseManagementSystem.Application.DTOs;

namespace WarehouseManagementSystem.Validator;

public class SaleCreateValidator : AbstractValidator<SaleCreateDto>
{
    public SaleCreateValidator()
    {
        RuleFor(s => s.ProductId)
            .GreaterThan(0);

        RuleFor(s => s.Quantity)
            .GreaterThan(0);
    }
}
