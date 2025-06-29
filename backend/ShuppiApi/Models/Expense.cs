using System.ComponentModel.DataAnnotations;

namespace ShuppiApi.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int MainCategory { get; set; }

        [Required]
        public int SubCategory { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string? Memo { get; set; }
    }
}
