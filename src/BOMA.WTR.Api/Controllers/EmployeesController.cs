using BOMA.WTR.Application.UseCases.Employees.Commands.Add;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

public class EmployeesController : ApiBaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Mediator.Send(new GetAllEmployeesQuery()));
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddEmployeeCommand command)
    {
        var result = await Mediator.Send(command);

        return CreatedAtAction(nameof(GetAll), new { id = result.EmployeeId }, new { result.EmployeeId });
    }
}