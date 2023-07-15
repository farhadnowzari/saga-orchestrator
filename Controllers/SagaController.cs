using MediatR;
using Microsoft.AspNetCore.Mvc;
using SagaOrchestrator.Commands;

namespace SagaOrchestrator.Controllers;

[ApiController]
[Route("saga")]
public class SagaController : ControllerBase
{
    private readonly IMediator _mediator;

    public SagaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("queue")]
    public async Task QueueProcess([FromBody] StoreProcessCommand request) => await _mediator.Send(request);
    [HttpPost("commit")]
    public async Task Commit([FromBody] CommitProcessCommand request) => await _mediator.Send(request);
}
