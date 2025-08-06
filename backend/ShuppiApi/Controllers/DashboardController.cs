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

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("monthly-summary")]
    public async Task<IActionResult> GetMonthlySummary([FromQuery] int? year, [FromQuery] int? month)
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdStr, out var userId))
            return BadRequest("Invalid user ID.");

        var now = DateTime.UtcNow;

        // 年月が指定されていなければ現在の年月を使用
        var targetYear = year ?? now.Year;
        var targetMonth = month ?? now.Month;

        var startOfMonth = new DateTime(targetYear, targetMonth, 1, 0, 0, 0, DateTimeKind.Utc);
        var startOfNextMonth = startOfMonth.AddMonths(1);

        var expenses = await _context.Expenses
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

        return Ok(new
        {
            totalAmount,
            categorySummaries
        });
    }
}
