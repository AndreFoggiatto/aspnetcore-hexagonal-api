using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Models;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Interfaces;

public interface IFeatureExampleDomainService
{
    Task<bool> ValidateBusinessRulesAsync(FeatureExampleEntity entity);
    Task<bool> CanBeDeletedAsync(int id);
    Task<bool> CanChangeStatusAsync(int id, Domain.Enums.FeatureExampleStatus newStatus);
}