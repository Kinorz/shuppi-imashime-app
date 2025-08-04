using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShuppiApi.Migrations
{
    /// <inheritdoc />
    public partial class InitWithTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_TagConcepts_ConceptId",
                table: "Tags");

            migrationBuilder.DropTable(
                name: "TagConcepts");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ConceptId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ConceptId",
                table: "Tags");

            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Tags",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "TagCategories",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagCategories", x => new { x.TagId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_TagCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagCategories_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TagCategories_CategoryId",
                table: "TagCategories",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TagCategories");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Tags");

            migrationBuilder.AddColumn<int>(
                name: "ConceptId",
                table: "Tags",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TagConcepts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Meaning = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagConcepts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ConceptId",
                table: "Tags",
                column: "ConceptId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_TagConcepts_ConceptId",
                table: "Tags",
                column: "ConceptId",
                principalTable: "TagConcepts",
                principalColumn: "Id");
        }
    }
}
