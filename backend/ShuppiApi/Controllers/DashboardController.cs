using System.Diagnostics;
using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShuppiApi.Data;

namespace ShuppiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<DashboardController> _log;

    public DashboardController(AppDbContext context, ILogger<DashboardController> log)
    {
        _context = context;
        _log = log;
    }

    [HttpGet("monthly-summary")]
    public async Task<IActionResult> GetMonthlySummary([FromQuery] int? year, [FromQuery] int? month)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
            return BadRequest("Invalid user ID.");

        var now = DateTime.UtcNow;

        var targetYear = year ?? now.Year;
        var targetMonth = month ?? now.Month;

        var startOfMonth = new DateTime(targetYear, targetMonth, 1, 0, 0, 0, DateTimeKind.Utc);
        var startOfNextMonth = startOfMonth.AddMonths(1);

        var expenses = await _context.Expenses
            .AsNoTracking()
            .Where(e => e.UserId == userId && e.Date >= startOfMonth && e.Date < startOfNextMonth)
            .Include(e => e.Category)
            .ToListAsync();

        var totalAmount = expenses.Sum(e => e.Amount);

        var categorySummaries = expenses
            .GroupBy(e => new { e.CategoryId, e.Category.Name })
            .Select(g => new
            {
                categoryId = g.Key.CategoryId,
                categoryName = g.Key.Name,
                amount = g.Sum(x => x.Amount)
            })
            .ToList();

        var result = Ok(new
        {
            totalAmount,
            categorySummaries
        });

        return result;
    }
}
