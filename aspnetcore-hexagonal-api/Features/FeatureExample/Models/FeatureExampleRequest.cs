using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Models;

public class CreateFeatureExampleRequest
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    [StringLength(500)]
    public required string Description { get; set; }

    public FeatureExampleStatus Status { get; set; } = FeatureExampleStatus.Active;
}

public class UpdateFeatureExampleRequest
{
    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [Required]
    [StringLength(500)]
    public required string Description { get; set; }

    public FeatureExampleStatus Status { get; set; }
}

public class FeatureExampleQueryRequest
{
    public string? Name { get; set; }
    public FeatureExampleStatus? Status { get; set; }
    public DateTime? CreatedFrom { get; set; }
    public DateTime? CreatedTo { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}