using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class ReportsController : ApiBaseController
{
    [HttpGet("workingTimeRecords")]
    public async Task<IActionResult> Get([FromQuery] int month, [FromQuery] int year, [FromQuery] int departmentId, [FromQuery] int shiftId, [FromQuery] string? searchText = null)
    {
        var queryModel = new GetRecordsQueryModel(month, year, departmentId, shiftId, searchText);
        
        // TODO: Dedicated query for reports
        return Ok(await Mediator.Send(new GetAllWorkingTimeRecordsQuery(queryModel)));
    }
}