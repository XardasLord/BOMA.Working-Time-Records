using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using FluentValidation;

namespace BOMA.WTR.Application.UseCases.Employees.Commands.Edit;

public sealed class EditEmployeeCommandValidator : AbstractValidator<EditEmployeeCommand>
{
    public EditEmployeeCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(64);
        RuleFor(x => x.RcpId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Position).MaximumLength(64);
        RuleFor(x => (ShiftType)x.ShiftTypeId).IsInEnum();
    }
}