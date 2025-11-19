using BOMA.WTR.Application.Abstractions.Messaging;
using BOMA.WTR.Application.UseCases.Reports.Models;

namespace BOMA.WTR.Application.UseCases.Reports.Queries;

public sealed record GetDataQuery(GetDataQueryModel QueryModel) : IQuery<ReportViewModel>;