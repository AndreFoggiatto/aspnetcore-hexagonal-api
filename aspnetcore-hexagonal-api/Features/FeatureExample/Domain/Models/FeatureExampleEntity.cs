using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Enums;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Models;

public class FeatureExampleEntity
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public FeatureExampleStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;
}