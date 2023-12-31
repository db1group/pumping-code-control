﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using PumpingControl.Application.Common.Enums;
using PumpingControl.Application.NationBehaviors.Commands;
using PumpingControl.Application.NationBehaviors.Contracts;
using PumpingControl.Application.NationBehaviors.Queries;

namespace PumpingControl.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NationController : ApiController
{
    private readonly ISender _mediator;

    public NationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllNations([FromQuery]OrderByParameter? orderBy = null)
    {
        var query = new GetAllNationsQuery(orderBy);
        var queryResult = await _mediator.Send(query);

        return queryResult.IsError ? Problem(queryResult.Errors) : Ok(queryResult.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNation(Guid id)
    {
        var query = new GetNationQuery(id);
        var queryResult = await _mediator.Send(query);

        return queryResult.IsError ? Problem(queryResult.Errors) : Ok(queryResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNewNation(NationRequest body)
    {
        var command = new CreateNewNationCommand(body.Name);
        var commandResult = await _mediator.Send(command);

        return commandResult.IsError
            ? Problem(commandResult.Errors)
            : CreatedAtAction(nameof(GetNation), new { id = commandResult.Value.Id}, commandResult.Value);
    }
}