using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ShuppiApi.Models;

public class TagCategory
{
  [Key, Column(Order = 0)]
  public int TagId { get; set; }

  public Tag Tag { get; set; } = null!;

  [Key, Column(Order = 1)]
  public int CategoryId { get; set; }
  
  public Category Category { get; set; } = null!;
}
