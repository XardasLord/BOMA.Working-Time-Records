using BOMA.WTR.Application.UseCases.Employees.Commands.Add;
using BOMA.WTR.Application.UseCases.Employees.Commands.Deactivate;
using BOMA.WTR.Application.UseCases.Employees.Commands.Edit;
using BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsDetailsData;
using BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsReportHoursData;
using BOMA.WTR.Application.UseCases.Employees.Commands.EditWorkingTimeRecordsSummaryData;
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

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Edit(int id, EditEmployeeCommand command)
    {
        command = command with
        {
            Id = id
        };
        
        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Deactivate(int id)
    {
        await Mediator.Send(new DeactivateEmployeeCommand(id));

        return NoContent();
    }

    [HttpPatch]
    [Route("{id}/workingTimeRecordsSummary")]
    public async Task<IActionResult> EditWorkingTimeRecordsSummaryData(int id, EditWorkingTimeRecordsSummaryDataCommand command)
    {
        command = command with
        {
            EmployeeId = id
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPatch]
    [Route("{id}/workingTimeRecordsDetails")]
    public async Task<IActionResult> EditWorkingTimeRecordsDetailsData(int id, EditWorkingTimeRecordsDetailsDataCommand command)
    {
        command = command with
        {
            EmployeeId = id
        };

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPatch]
    [Route("{id}/workingTimeRecordsReportHours")]
    public async Task<IActionResult> EditWorkingTimeRecordsReportHoursData(int id, EditWorkingTimeRecordsReportHoursDataCommand command)
    {
        command = command with
        {
            EmployeeId = id
        };

        await Mediator.Send(command);

        return NoContent();
    }
}