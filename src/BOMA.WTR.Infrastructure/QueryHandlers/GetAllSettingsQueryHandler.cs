using AutoMapper;
using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Settings.Queries.GetAll;
using BOMA.WTR.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BOMA.WTR.Infrastructure.QueryHandlers;

public sealed class GetAllSettingsQueryHandler(BomaDbContext dbContext, IMapper mapper)
    : IQueryHandler<GetAllSettingsQuery, IEnumerable<SettingViewModel>>
{
    public async Task<IEnumerable<SettingViewModel>> Handle(GetAllSettingsQuery query, CancellationToken cancellationToken)
    {
        var settings = await dbContext.Settings.ToListAsync(cancellationToken);

        return mapper.Map<IEnumerable<SettingViewModel>>(settings);
    }
}