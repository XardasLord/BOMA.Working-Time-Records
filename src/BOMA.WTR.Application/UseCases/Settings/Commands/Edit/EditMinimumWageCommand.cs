using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Settings.Commands.Edit;

public sealed record EditMinimumWageCommand(int MinimumWage) : ICommand;