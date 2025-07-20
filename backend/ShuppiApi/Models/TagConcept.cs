using System.ComponentModel.DataAnnotations;

namespace ShuppiApi.Models;

public class TagConcept
{
  [Key]
  public int Id { get; set; }

  [Required]
  [MaxLength(100)]
  public string Meaning { get; set; } = null!;

  public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
