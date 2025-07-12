using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands.CloseMonth;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class WorkingTimeRecordsController : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int month, [FromQuery] int year, [FromQuery] int departmentId, [FromQuery] int shiftId, [FromQuery] string? searchText = null)
    {
        var queryModel = new GetRecordsQueryModel(month, year, departmentId, shiftId, searchText);
        return Ok(await Mediator.Send(new GetAllWorkingTimeRecordsQuery(queryModel)));
    }

    [HttpPost]
    [Route("close-month")]
    public async Task<IActionResult> CloseMonth()
    {
        await Mediator.Send(new CloseMonthCommand());

        return NoContent();
    }
}