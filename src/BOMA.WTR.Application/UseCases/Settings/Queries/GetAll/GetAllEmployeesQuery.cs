using BOMA.WTR.Application.Abstractions.Messaging;

namespace BOMA.WTR.Application.UseCases.Settings.Queries.GetAll;

public sealed record GetAllSettingsQuery : IQuery<IEnumerable<SettingViewModel>>;