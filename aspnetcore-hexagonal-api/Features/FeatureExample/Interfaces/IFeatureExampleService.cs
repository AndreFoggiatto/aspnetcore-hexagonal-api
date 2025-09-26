using aspnetcore_hexagonal_api.Features.FeatureExample.Models;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Interfaces;

public interface IFeatureExampleService
{
    Task<ApiResponse<FeatureExampleResponse>> CreateAsync(CreateFeatureExampleRequest request, string? createdBy = null);
    Task<ApiResponse<FeatureExampleResponse>> GetByIdAsync(int id);
    Task<ApiResponse<PagedFeatureExampleResponse>> GetAllAsync(FeatureExampleQueryRequest request);
    Task<ApiResponse<FeatureExampleResponse>> UpdateAsync(int id, UpdateFeatureExampleRequest request, string? updatedBy = null);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}

public interface IFeatureExampleRepository
{
    Task<Domain.Models.FeatureExampleEntity?> GetByIdAsync(int id);
    Task<Domain.Models.FeatureExampleEntity> CreateAsync(Domain.Models.FeatureExampleEntity entity);
    Task<Domain.Models.FeatureExampleEntity> UpdateAsync(Domain.Models.FeatureExampleEntity entity);
    Task<bool> DeleteAsync(int id);
    Task<(IEnumerable<Domain.Models.FeatureExampleEntity> Items, int TotalCount)> GetAllAsync(FeatureExampleQueryRequest request);
    Task<bool> ExistsAsync(int id);
}