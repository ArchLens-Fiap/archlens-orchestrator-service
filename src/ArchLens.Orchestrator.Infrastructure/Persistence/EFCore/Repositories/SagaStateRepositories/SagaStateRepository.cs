using ArchLens.Orchestrator.Application.Contracts.DTOs.SagaDTOs;
using ArchLens.Orchestrator.Application.Contracts.Interfaces;
using ArchLens.Orchestrator.Infrastructure.Persistence.EFCore.Context;
using ArchLens.Orchestrator.Infrastructure.Saga;
using Microsoft.EntityFrameworkCore;

namespace ArchLens.Orchestrator.Infrastructure.Persistence.EFCore.Repositories.SagaStateRepositories;

public sealed class SagaStateRepository(SagaDbContext dbContext) : ISagaStateRepository
{
    public async Task<SagaStatusResponse?> GetByDiagramIdAsync(Guid diagramId, CancellationToken ct = default)
    {
        var state = await dbContext.SagaStates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.DiagramId == diagramId, ct);

        return state is null ? null : MapToResponse(state);
    }

    public async Task<SagaStatusResponse?> GetByAnalysisIdAsync(Guid analysisId, CancellationToken ct = default)
    {
        var state = await dbContext.SagaStates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.AnalysisId == analysisId, ct);

        return state is null ? null : MapToResponse(state);
    }

    public async Task<IReadOnlyList<SagaStatusResponse>> ListAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var states = await dbContext.SagaStates
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return states.Select(MapToResponse).ToList();
    }

    public async Task<bool> DeleteByDiagramIdAsync(Guid diagramId, CancellationToken ct = default)
    {
        var state = await dbContext.SagaStates
            .FirstOrDefaultAsync(x => x.DiagramId == diagramId, ct);

        if (state is null) return false;

        dbContext.SagaStates.Remove(state);
        await dbContext.SaveChangesAsync(ct);
        return true;
    }

    private static SagaStatusResponse MapToResponse(AnalysisSagaState state) => new(
        state.CorrelationId,
        state.DiagramId,
        state.AnalysisId,
        state.CurrentState,
        state.FileName,
        state.RetryCount,
        state.ErrorMessage,
        state.ReportId,
        state.ProcessingTimeMs,
        state.CreatedAt,
        state.UpdatedAt);
}
