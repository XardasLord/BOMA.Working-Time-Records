using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class WorkingTimeRecordsController : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int month, [FromQuery] int year, [FromQuery] int groupId)
    {
        return Ok(await Mediator.Send(new GetAllWorkingTimeRecordsQuery(month, year, groupId)));
    }
    
}