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

  public int? ConceptId { get; set; }

  public TagConcept? Concept { get; set; }

  public ICollection<ExpenseTag> ExpenseTags { get; set; } = new List<ExpenseTag>();
}
