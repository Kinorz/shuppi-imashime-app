using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShuppiApi.Data;
using ShuppiApi.Models;

namespace ShuppiApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpenseController : ControllerBase
{
  private readonly AppDbContext _context;

  public ExpenseController(AppDbContext context)
  {
    _context = context;
  }

  [HttpPost]
  public async Task<IActionResult> CreateExpense([FromBody] ExpenseCreateDto dto)
  {
    var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out var userId))
            return BadRequest("Invalid user id.");

    // 支出エンティティ作成
    var expense = new Expense
    {
      Amount = dto.Amount,
      Date = dto.Date,
      CategoryId = dto.CategoryId,
      UserId = userId,
      Note = dto.Note,
    };

    // UTC変換
    if (expense.Date.Kind == DateTimeKind.Unspecified)
    {
        expense.Date = DateTime.SpecifyKind(expense.Date, DateTimeKind.Utc);
    }

    // タグ名をユニークに
    var tagNames = dto.Tags.Distinct().ToList();

    // 既存のタグを取得
    var existingTags = await _context.Tags
        .Where(t => t.UserId == userId && tagNames.Contains(t.Name))
        .ToListAsync();

    // DBに存在しないタグ名を抽出
    var existingNames = existingTags.Select(t => t.Name).ToHashSet();
    var newTagNames = tagNames.Where(name => !existingNames.Contains(name)).ToList();

    // 新規タグを作成
    var newTags = newTagNames.Select(name => new Tag
    {
        Name = name,
        UserId = userId,
        IsHidden = false,
        TagCategories = new List<TagCategory>
        {
            new TagCategory { CategoryId = dto.CategoryId }
        }
    }).ToList();

    // DBに新規タグを追加
    _context.Tags.AddRange(newTags);
    await _context.SaveChangesAsync();

    // 全タグ（既存＋新規）をまとめてExpenseに紐づけ
    var allTags = existingTags.Concat(newTags);
    expense.ExpenseTags = allTags.Select(tag => new ExpenseTag
    {
        TagId = tag.Id
    }).ToList();

    // タグとカテゴリの関係を追加
    // 既存のタグIDを取得
    var tagIds = allTags.Select(t => t.Id).ToList();

    // 既存のタグとカテゴリの関係を取得
    var existingTagCategories = await _context.TagCategories
        .Where(tc => tagIds.Contains(tc.TagId) && tc.CategoryId == dto.CategoryId)
        .Select(tc => new { tc.TagId, tc.CategoryId })
        .ToListAsync();
    var existingPairs = existingTagCategories
        .Select(tc => (tc.TagId, tc.CategoryId))
        .ToHashSet();
    
    // 新規のタグとカテゴリの関係を作成
    var newTagCategories = allTags
        .Where(tag => !existingPairs.Contains((tag.Id, dto.CategoryId)))
        .Select(tag => new TagCategory
        {
            TagId = tag.Id,
            CategoryId = dto.CategoryId
        })
        .ToList();

    // 新規のタグとカテゴリの関係をDBに追加
    _context.TagCategories.AddRange(newTagCategories);
    // 支出をDBに追加
    _context.Expenses.Add(expense);
    await _context.SaveChangesAsync();

    var response = new ExpenseResponseDto
    {
        Id = expense.Id,
        Amount = expense.Amount,
        Date = expense.Date,
        CategoryId = expense.CategoryId,
        Note = expense.Note,
        Tags = allTags.Select(t => t.Name).ToList()
    };

    return Ok(response);
  }
}
