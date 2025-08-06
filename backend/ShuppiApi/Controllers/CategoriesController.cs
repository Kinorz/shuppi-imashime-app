using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShuppiApi.Data;
using ShuppiApi.Models;

namespace ShuppiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        return await _context.Categories.OrderBy(c => c.Id).ToListAsync();
    }
}
