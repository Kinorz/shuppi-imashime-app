using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ShuppiApi.Models;

namespace ShuppiApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<ExpenseTag> ExpenseTags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<TagCategory> TagCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ExpenseTag: 複合主キー
        modelBuilder.Entity<ExpenseTag>()
            .HasKey(et => new { et.ExpenseId, et.TagId });

        modelBuilder.Entity<ExpenseTag>()
            .HasOne(et => et.Expense)
            .WithMany(e => e.ExpenseTags)
            .HasForeignKey(et => et.ExpenseId);

        modelBuilder.Entity<ExpenseTag>()
            .HasOne(et => et.Tag)
            .WithMany(t => t.ExpenseTags)
            .HasForeignKey(et => et.TagId);

        modelBuilder.Entity<TagCategory>()
            .HasKey(tc => new { tc.TagId, tc.CategoryId });

        modelBuilder.Entity<TagCategory>()
            .HasOne(tc => tc.Tag)
            .WithMany(t => t.TagCategories)
            .HasForeignKey(tc => tc.TagId);

        modelBuilder.Entity<TagCategory>()
            .HasOne(tc => tc.Category)
            .WithMany(c => c.TagCategories)
            .HasForeignKey(tc => tc.CategoryId);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "食費", Color = "#f28b82", Icon = "sym_o_restaurant" },
            new Category { Id = 2, Name = "日用品", Color = "#fbbc04", Icon = "sym_o_shopping_basket" },
            new Category { Id = 3, Name = "衣類・美容", Color = "#c58af9", Icon = "sym_o_styler" },
            new Category { Id = 4, Name = "娯楽費", Color = "#81d4fa", Icon = "sym_o_sports_esports" },
            new Category { Id = 5, Name = "固定費", Color = "#9aa0a6", Icon = "sym_o_home" },
            new Category { Id = 6, Name = "その他", Color = "#d7ccc8", Icon = "sym_o_category" }
        );
    }

}
