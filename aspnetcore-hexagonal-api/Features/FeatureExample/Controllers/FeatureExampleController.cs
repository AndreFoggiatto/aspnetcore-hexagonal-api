using aspnetcore_hexagonal_api.Features.FeatureExample.Interfaces;
using aspnetcore_hexagonal_api.Features.FeatureExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class FeatureExampleController : ControllerBase
{
    private readonly IFeatureExampleService _featureExampleService;
    private readonly ILogger<FeatureExampleController> _logger;

    public FeatureExampleController(
        IFeatureExampleService featureExampleService,
        ILogger<FeatureExampleController> logger)
    {
        _featureExampleService = featureExampleService;
        _logger = logger;
    }

    /// <summary>
    /// Creates a new feature example
    /// </summary>
    /// <param name="request">The feature example creation request</param>
    /// <returns>The created feature example</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FeatureExampleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<FeatureExampleResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateFeatureExampleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdBy = User?.Identity?.Name;
        var result = await _featureExampleService.CreateAsync(request, createdBy);

        if (result.Success)
        {
            return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Gets a feature example by its ID
    /// </summary>
    /// <param name="id">The feature example ID</param>
    /// <returns>The feature example</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<FeatureExampleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FeatureExampleResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _featureExampleService.GetByIdAsync(id);

        if (result.Success)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

    /// <summary>
    /// Gets all feature examples with optional filtering and pagination
    /// </summary>
    /// <param name="request">The query parameters</param>
    /// <returns>A paginated list of feature examples</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedFeatureExampleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll([FromQuery] FeatureExampleQueryRequest request)
    {
        var result = await _featureExampleService.GetAllAsync(request);

        if (result.Success)
        {
            return Ok(result);
        }

        return StatusCode(500, result);
    }

    /// <summary>
    /// Updates an existing feature example
    /// </summary>
    /// <param name="id">The feature example ID</param>
    /// <param name="request">The update request</param>
    /// <returns>The updated feature example</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<FeatureExampleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<FeatureExampleResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<FeatureExampleResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateFeatureExampleRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedBy = User?.Identity?.Name;
        var result = await _featureExampleService.UpdateAsync(id, request, updatedBy);

        if (result.Success)
        {
            return Ok(result);
        }

        if (result.Message.Contains("not found"))
        {
            return NotFound(result);
        }

        return BadRequest(result);
    }

    /// <summary>
    /// Soft deletes a feature example
    /// </summary>
    /// <param name="id">The feature example ID</param>
    /// <returns>Deletion result</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _featureExampleService.DeleteAsync(id);

        if (result.Success)
        {
            return Ok(result);
        }

        if (result.Message.Contains("not found"))
        {
            return NotFound(result);
        }

        return BadRequest(result);
    }
}