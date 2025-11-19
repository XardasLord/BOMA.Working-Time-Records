using BOMA.WTR.Application.UseCases.Reports.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class ReportsController : ApiBaseController
{
    [HttpGet("workingTimeRecords")]
    public async Task<IActionResult> Get([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int departmentId, [FromQuery] int shiftId, [FromQuery] string? searchText = null)
    {
        var queryModel = new GetDataQueryModel(startDate, endDate, departmentId, shiftId, searchText);
        
        return Ok(await Mediator.Send(new GetDataQuery(queryModel)));
    }
}