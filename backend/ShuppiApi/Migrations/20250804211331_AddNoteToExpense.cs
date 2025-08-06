﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShuppiApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteToExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Expenses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Expenses");
        }
    }
}
