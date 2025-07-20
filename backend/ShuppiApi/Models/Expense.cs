using System.ComponentModel.DataAnnotations;

namespace ShuppiApi.Models;

public class Expense
{
    [Key]
    public int Id { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;

    [Required]
    public int UserId { get; set; }

    public User User { get; set; } = null!;

    public ICollection<ExpenseTag> ExpenseTags { get; set; } = new List<ExpenseTag>();
}
