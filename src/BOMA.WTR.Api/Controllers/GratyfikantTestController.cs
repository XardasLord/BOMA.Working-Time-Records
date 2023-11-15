using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class GratyfikantTestController : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> SendToGratyfikant([FromQuery] GetRecordsQueryModel queryModel)
    {
        await Mediator.Send(new SetWorkingTimeRecordsInGratyfikantCommand(queryModel));

        return NoContent();
    }
}