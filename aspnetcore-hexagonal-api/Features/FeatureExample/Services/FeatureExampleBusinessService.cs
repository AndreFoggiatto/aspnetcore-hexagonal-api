using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Enums;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Interfaces;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Models;
using aspnetcore_hexagonal_api.Features.FeatureExample.Interfaces;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Services;

public class FeatureExampleBusinessService : IFeatureExampleDomainService
{
    private readonly IFeatureExampleRepository _repository;
    private readonly ILogger<FeatureExampleBusinessService> _logger;

    public FeatureExampleBusinessService(
        IFeatureExampleRepository repository,
        ILogger<FeatureExampleBusinessService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<bool> ValidateBusinessRulesAsync(FeatureExampleEntity entity)
    {
        try
        {
            await Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(entity.Name?.Trim()))
            {
                _logger.LogWarning("Validation failed: Name is empty");
                return false;
            }

            if (string.IsNullOrWhiteSpace(entity.Description?.Trim()))
            {
                _logger.LogWarning("Validation failed: Description is empty");
                return false;
            }

            if (entity.Name.Length < 3)
            {
                _logger.LogWarning("Validation failed: Name too short");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating business rules for entity");
            return false;
        }
    }

    public async Task<bool> CanBeDeletedAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            return entity.Status != FeatureExampleStatus.Active;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if entity can be deleted: {Id}", id);
            return false;
        }
    }

    public async Task<bool> CanChangeStatusAsync(int id, FeatureExampleStatus newStatus)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            return (entity.Status, newStatus) switch
            {
                (FeatureExampleStatus.Inactive, FeatureExampleStatus.Active) => true,
                (FeatureExampleStatus.Active, FeatureExampleStatus.Pending) => true,
                (FeatureExampleStatus.Active, FeatureExampleStatus.Inactive) => true,
                (FeatureExampleStatus.Pending, FeatureExampleStatus.Active) => true,
                (FeatureExampleStatus.Pending, FeatureExampleStatus.Completed) => true,
                (FeatureExampleStatus.Pending, FeatureExampleStatus.Cancelled) => true,
                (FeatureExampleStatus.Completed, FeatureExampleStatus.Active) => false,
                (FeatureExampleStatus.Cancelled, _) => false,
                _ => false
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if status can be changed for entity: {Id}", id);
            return false;
        }
    }
}