using aspnetcore_hexagonal_api.Features.FeatureExample.Application;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Enums;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Interfaces;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Models;
using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Validators;
using aspnetcore_hexagonal_api.Features.FeatureExample.Interfaces;
using aspnetcore_hexagonal_api.Features.FeatureExample.Models;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Application;

public class FeatureExampleService : IFeatureExampleService
{
    private readonly IFeatureExampleRepository _repository;
    private readonly IFeatureExampleDomainService _domainService;
    private readonly ILogger<FeatureExampleService> _logger;

    public FeatureExampleService(
        IFeatureExampleRepository repository,
        IFeatureExampleDomainService domainService,
        ILogger<FeatureExampleService> logger)
    {
        _repository = repository;
        _domainService = domainService;
        _logger = logger;
    }

    public async Task<ApiResponse<FeatureExampleResponse>> CreateAsync(CreateFeatureExampleRequest request, string? createdBy = null)
    {
        try
        {
            var entity = new FeatureExampleEntity
            {
                Name = request.Name.Trim(),
                Description = request.Description.Trim(),
                Status = request.Status,
                CreatedBy = createdBy
            };

            var validator = new FeatureExampleValidator();
            var validationResult = await validator.ValidateAsync(entity);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<FeatureExampleResponse>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var businessValidation = await _domainService.ValidateBusinessRulesAsync(entity);
            if (!businessValidation)
            {
                return new ApiResponse<FeatureExampleResponse>
                {
                    Success = false,
                    Message = "Business validation failed"
                };
            }

            var created = await _repository.CreateAsync(entity);
            var response = MapToResponse(created);

            return new ApiResponse<FeatureExampleResponse>
            {
                Success = true,
                Message = "Feature example created successfully",
                Data = response
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating feature example");
            return new ApiResponse<FeatureExampleResponse>
            {
                Success = false,
                Message = "An error occurred while creating the feature example"
            };
        }
    }

    public async Task<ApiResponse<FeatureExampleResponse>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                return new ApiResponse<FeatureExampleResponse>
                {
                    Success = false,
                    Message = FeatureExampleConstants.ValidationMessages.EntityNotFound
                };
            }

            var response = MapToResponse(entity);

            return new ApiResponse<FeatureExampleResponse>
            {
                Success = true,
                Data = response
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting feature example by id: {Id}", id);
            return new ApiResponse<FeatureExampleResponse>
            {
                Success = false,
                Message = "An error occurred while retrieving the feature example"
            };
        }
    }

    public async Task<ApiResponse<PagedFeatureExampleResponse>> GetAllAsync(FeatureExampleQueryRequest request)
    {
        try
        {
            request.PageSize = Math.Min(request.PageSize, FeatureExampleConstants.DefaultValues.MaxPageSize);

            var (items, totalCount) = await _repository.GetAllAsync(request);

            var responses = items.Select(MapToResponse).ToList();
            var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

            var pagedResponse = new PagedFeatureExampleResponse
            {
                Items = responses,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                HasNextPage = request.Page < totalPages,
                HasPreviousPage = request.Page > 1
            };

            return new ApiResponse<PagedFeatureExampleResponse>
            {
                Success = true,
                Data = pagedResponse
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting feature examples");
            return new ApiResponse<PagedFeatureExampleResponse>
            {
                Success = false,
                Message = "An error occurred while retrieving feature examples"
            };
        }
    }

    public async Task<ApiResponse<FeatureExampleResponse>> UpdateAsync(int id, UpdateFeatureExampleRequest request, string? updatedBy = null)
    {
        try
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                return new ApiResponse<FeatureExampleResponse>
                {
                    Success = false,
                    Message = FeatureExampleConstants.ValidationMessages.EntityNotFound
                };
            }

            var canChangeStatus = await _domainService.CanChangeStatusAsync(id, request.Status);
            if (!canChangeStatus)
            {
                return new ApiResponse<FeatureExampleResponse>
                {
                    Success = false,
                    Message = FeatureExampleConstants.ValidationMessages.CannotChangeStatus
                };
            }

            existing.Name = request.Name.Trim();
            existing.Description = request.Description.Trim();
            existing.Status = request.Status;
            existing.UpdatedBy = updatedBy;

            var validator = new FeatureExampleValidator();
            var validationResult = await validator.ValidateAsync(existing);

            if (!validationResult.IsValid)
            {
                return new ApiResponse<FeatureExampleResponse>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList()
                };
            }

            var updated = await _repository.UpdateAsync(existing);
            var response = MapToResponse(updated);

            return new ApiResponse<FeatureExampleResponse>
            {
                Success = true,
                Message = "Feature example updated successfully",
                Data = response
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating feature example: {Id}", id);
            return new ApiResponse<FeatureExampleResponse>
            {
                Success = false,
                Message = "An error occurred while updating the feature example"
            };
        }
    }

    public async Task<ApiResponse<bool>> DeleteAsync(int id)
    {
        try
        {
            var exists = await _repository.ExistsAsync(id);
            if (!exists)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = FeatureExampleConstants.ValidationMessages.EntityNotFound
                };
            }

            var canDelete = await _domainService.CanBeDeletedAsync(id);
            if (!canDelete)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = FeatureExampleConstants.ValidationMessages.CannotDelete
                };
            }

            var deleted = await _repository.DeleteAsync(id);

            return new ApiResponse<bool>
            {
                Success = deleted,
                Message = deleted ? "Feature example deleted successfully" : "Failed to delete feature example",
                Data = deleted
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting feature example: {Id}", id);
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "An error occurred while deleting the feature example"
            };
        }
    }

    private static FeatureExampleResponse MapToResponse(FeatureExampleEntity entity)
    {
        return new FeatureExampleResponse
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            Status = entity.Status,
            StatusDescription = GetStatusDescription(entity.Status),
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            CreatedBy = entity.CreatedBy,
            UpdatedBy = entity.UpdatedBy,
            IsActive = entity.IsActive
        };
    }

    private static string GetStatusDescription(FeatureExampleStatus status)
    {
        return status switch
        {
            FeatureExampleStatus.Inactive => FeatureExampleConstants.StatusDescriptions.Inactive,
            FeatureExampleStatus.Active => FeatureExampleConstants.StatusDescriptions.Active,
            FeatureExampleStatus.Pending => FeatureExampleConstants.StatusDescriptions.Pending,
            FeatureExampleStatus.Completed => FeatureExampleConstants.StatusDescriptions.Completed,
            FeatureExampleStatus.Cancelled => FeatureExampleConstants.StatusDescriptions.Cancelled,
            _ => "Unknown"
        };
    }
}