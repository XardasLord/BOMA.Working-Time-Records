﻿using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class WorkingTimeRecordHistoriesController : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int month, [FromQuery] int year, [FromQuery] int departmentId, [FromQuery] string? searchText = null)
    {
        var queryModel = new GetRecordsQueryModel(month, year, departmentId, searchText);
        return Ok(await Mediator.Send(new GetAllWorkingTimeRecordHistoriesQuery(queryModel)));
    }
}