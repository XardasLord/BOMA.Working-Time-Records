﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BOMA.WTR.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ApiBaseController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>() 
                                                  ?? throw new NotImplementedException("Cannot get IMediator service");
}