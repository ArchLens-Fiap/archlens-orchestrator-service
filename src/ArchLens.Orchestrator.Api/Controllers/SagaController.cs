using ArchLens.Orchestrator.Application.UseCases.Sagas.Queries.GetStatus;
using ArchLens.Orchestrator.Application.UseCases.Sagas.Queries.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArchLens.Orchestrator.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public sealed class SagaController(IMediator mediator) : ControllerBase
{
    [HttpGet("diagram/{diagramId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByDiagram(Guid diagramId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSagaStatusByDiagramQuery(diagramId), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet("analysis/{analysisId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByAnalysis(Guid analysisId, CancellationToken ct)
    {
        var result = await mediator.Send(new GetSagaStatusByAnalysisQuery(analysisId), ct);
        return result.IsSuccess ? Ok(result.Value) : NotFound();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var result = await mediator.Send(new ListSagasQuery(page, pageSize), ct);
        return Ok(result.Value);
    }
}
