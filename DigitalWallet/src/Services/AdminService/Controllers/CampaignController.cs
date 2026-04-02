using System.Security.Claims;
using AdminService.Application.DTOs;
using AdminService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedContracts.DTOs;

namespace AdminService.Controllers;

[ApiController]
[Route("api/admin/campaigns")]
[Authorize(Roles = "Admin")]
public class CampaignController : ControllerBase
{
    private readonly ICampaignService _svc;
    public CampaignController(ICampaignService svc) => _svc = svc;

    private Guid GetAdminId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub")!);

    /// <summary>List all campaigns (paginated). Optionally filter active-only.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int size = 20,
        [FromQuery] bool? activeOnly = null)
    {
        var result = await _svc.GetAllAsync(page, size, activeOnly);
        return Ok(ApiResponse<PaginatedResult<CampaignDto>>.Ok(result));
    }

    /// <summary>Get a campaign by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _svc.GetByIdAsync(id);
        return Ok(ApiResponse<CampaignDto>.Ok(result));
    }

    /// <summary>Create a new campaign.</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCampaignRequest request)
    {
        var result = await _svc.CreateAsync(GetAdminId(), request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id },
            ApiResponse<CampaignDto>.Ok(result, "Campaign created."));
    }

    /// <summary>Update an existing campaign.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCampaignRequest request)
    {
        var result = await _svc.UpdateAsync(id, GetAdminId(), request);
        return Ok(ApiResponse<CampaignDto>.Ok(result, "Campaign updated."));
    }

    /// <summary>Delete a campaign.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _svc.DeleteAsync(id, GetAdminId());
        return Ok(ApiResponse<object>.Ok(null!, "Campaign deleted."));
    }
}
