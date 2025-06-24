using BOMA.WTR.Application.UseCases.Settings.Commands.Update;
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

    [HttpPatch]
    public async Task<IActionResult> UpdateSettings(UpdateCommand command)
    {
        await Mediator.Send(command);

        return NoContent();
    }
}