public class ExpenseResponseDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int CategoryId { get; set; }
    public string? Note { get; set; }
    public List<string> Tags { get; set; } = new();
}
