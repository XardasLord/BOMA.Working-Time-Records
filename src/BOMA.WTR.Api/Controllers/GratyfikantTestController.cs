using BOMA.WTR.Application.InsertGT.Gratyfikant;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class GratyfikantTestController : ApiBaseController
{
    private readonly IGratyfikantService _gratyfikantService;

    public GratyfikantTestController(IGratyfikantService gratyfikantService)
    {
        _gratyfikantService = gratyfikantService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        await _gratyfikantService.SetWorkingHours();

        return Ok();
    }
}