using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShuppiApi.Models;

public class ExpenseTag
{
  [Key, Column(Order = 0)]
  public int ExpenseId { get; set; }

  public Expense Expense { get; set; } = null!;

  [Key, Column(Order = 1)]
  public int TagId { get; set; }
  
  public Tag Tag { get; set; } = null!;
}
