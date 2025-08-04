using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShuppiApi.Models;

public class Tag
{
  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(50)]
  public string Name { get; set; } = null!;

  [Required]
  public int UserId { get; set; }

  public User User { get; set; } = null!;

  public bool IsHidden { get; set; } = false;

  public ICollection<ExpenseTag> ExpenseTags { get; set; } = new List<ExpenseTag>();
  public ICollection<TagCategory> TagCategories { get; set; } = new List<TagCategory>();
}
