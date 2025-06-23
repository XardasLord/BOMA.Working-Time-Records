using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.Exceptions;
using BOMA.WTR.Domain.AggregateModels.Setting;
using BOMA.WTR.Domain.AggregateModels.Setting.Specifications;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Application.UseCases.Settings.Commands.Edit;

public sealed class EditMinimumWageCommandHandler(IAggregateRepository<Setting> settingRepository)
    : ICommandHandler<EditMinimumWageCommand>
{
    private const string MinimumWageKey = "MinimumWage";
    
    public async Task Handle(EditMinimumWageCommand command, CancellationToken cancellationToken)
    {
        var setting = await settingRepository.SingleOrDefaultAsync(new SettingByKeySpec(MinimumWageKey), cancellationToken) 
            ?? throw new NotFoundException($"Setting with KEY = {MinimumWageKey} was not found");
        
        setting.Set(MinimumWageKey, command.MinimumWage.ToString(), "int", "Minimalna płaca pracownika");

        await settingRepository.SaveChangesAsync(cancellationToken);
    }
}