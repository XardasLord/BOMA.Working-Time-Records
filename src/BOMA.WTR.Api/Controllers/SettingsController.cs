using BOMA.WTR.Application.UseCases.Settings.Commands.Edit;
using BOMA.WTR.Application.UseCases.Settings.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class SettingsController : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllSettingsQuery()));
    }

    [HttpPut]
    [Route("minimum-wage")]
    public async Task<IActionResult> EditMinimumWage(int id, EditMinimumWageCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}