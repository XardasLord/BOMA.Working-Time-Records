using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Settings.Commands.Update;

public sealed record UpdateCommand(IEnumerable<UpdateSettingDto> Settings) : ICommand;

public sealed record UpdateSettingDto(string Key, string Value);