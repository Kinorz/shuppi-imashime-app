using System.ComponentModel.DataAnnotations;

namespace ShuppiApi.Models;

public class Category
{
  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(20)]
  public string Name { get; set; } = null!;

  [Required]
  [MaxLength(28)]
  public string Color { get; set; } = "#cccccc";

  [Required]
  [MaxLength(50)]
  public string Icon { get; set; } = "sym_o_category";

  public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
  public ICollection<TagCategory> TagCategories { get; set; } = new List<TagCategory>();
}
