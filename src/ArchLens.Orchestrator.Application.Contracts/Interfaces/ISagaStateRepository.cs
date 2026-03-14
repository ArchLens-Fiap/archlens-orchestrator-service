using ArchLens.Orchestrator.Application.Contracts.DTOs.SagaDTOs;

namespace ArchLens.Orchestrator.Application.Contracts.Interfaces;

public interface ISagaStateRepository
{
    Task<SagaStatusResponse?> GetByDiagramIdAsync(Guid diagramId, CancellationToken ct = default);
    Task<SagaStatusResponse?> GetByAnalysisIdAsync(Guid analysisId, CancellationToken ct = default);
    Task<IReadOnlyList<SagaStatusResponse>> ListAsync(int page, int pageSize, CancellationToken ct = default);
    Task<bool> DeleteByDiagramIdAsync(Guid diagramId, CancellationToken ct = default);
}
