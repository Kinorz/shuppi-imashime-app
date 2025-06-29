using Microsoft.EntityFrameworkCore;
using ShuppiApi.Models;

namespace ShuppiApi.Data;

public class ExpenseContext : DbContext
{
    public ExpenseContext(DbContextOptions<ExpenseContext> options)
        : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; }
}
