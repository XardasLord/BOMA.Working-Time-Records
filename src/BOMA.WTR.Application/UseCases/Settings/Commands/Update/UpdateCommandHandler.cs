using System.Text.Json;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels.Setting;
using BOMA.WTR.Domain.AggregateModels.Setting.Specifications;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.Settings.Commands.Update;

public sealed class UpdateCommandHandler(IAggregateRepository<Setting> settingRepository)
    : ICommandHandler<UpdateCommand>
{
    public async Task Handle(UpdateCommand command, CancellationToken cancellationToken)
    {
        foreach (var dto in command.Settings)
        {
            var setting = await settingRepository.SingleOrDefaultAsync(new SettingByKeySpec(dto.Key), cancellationToken);

            if (setting is null)
                throw new NotFoundException($"Setting with KEY = {dto.Key} was not found");

            setting.Set(dto.Key, dto.Value, InferType(dto.Value), setting.Description);
        }

        await settingRepository.SaveChangesAsync(cancellationToken);
    }
    
    private static string InferType(string value)
    {
        if (bool.TryParse(value, out _)) return "bool";
        if (int.TryParse(value, out _)) return "int";
        if (decimal.TryParse(value, out _)) return "decimal";
        try
        {
            JsonSerializer.Deserialize<object>(value);
            return "json";
        }
        catch
        {
            return "string";
        }
    }
}