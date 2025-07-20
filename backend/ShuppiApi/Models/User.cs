using System.ComponentModel.DataAnnotations;

namespace ShuppiApi.Models;

public class User
{
  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Email { get; set; } = null!;

  [Required]
  public string PasswordHash { get; set; } = null!;

  public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
  public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}

