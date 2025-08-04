using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ShuppiApi.Data;
using ShuppiApi.Dtos;
using System.Security.Claims;

namespace ShuppiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagsController : ControllerBase
{
    private readonly AppDbContext _context;

    public TagsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<TagDto>>> GetTags()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out var userId))
            return BadRequest("Invalid user id.");

        var tags = await _context.Tags
            .Where(t => t.UserId == userId)
            .Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name,
                IsHidden = t.IsHidden,
                CategoryIds = t.TagCategories.Select(tc => tc.CategoryId).ToList()
            })
            .ToListAsync();

        return Ok(tags);
    }
}
