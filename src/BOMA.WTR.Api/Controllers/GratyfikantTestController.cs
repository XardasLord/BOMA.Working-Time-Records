using BOMA.WTR.Application.InsertGT.Gratyfikant;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Commands;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class GratyfikantController : ApiBaseController
{
    private readonly IGratyfikantService _gratyfikantService;

    public GratyfikantController(IGratyfikantService gratyfikantService)
    {
        _gratyfikantService = gratyfikantService;
    }
    
    [HttpGet("test")]
    public async Task<IActionResult> Test([FromQuery] GetRecordsQueryModel queryModel)
    {
        await _gratyfikantService.OpenGratyfikant();
        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> SendToGratyfikant([FromQuery] GetRecordsQueryModel queryModel)
    {
        await Mediator.Send(new SetWorkingTimeRecordsInGratyfikantCommand(queryModel));

        return NoContent();
    }
}