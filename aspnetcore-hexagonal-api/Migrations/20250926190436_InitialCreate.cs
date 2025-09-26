using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace aspnetcore_hexagonal_api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeatureExamples",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureExamples", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FeatureExamples",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "IsActive", "Name", "Status", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Este é um exemplo inicial para demonstrar a funcionalidade", true, "Exemplo Inicial", 1, null, null },
                    { 2, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), "System", "Este exemplo está em status pendente", true, "Exemplo Pendente", 2, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeatureExamples_CreatedAt",
                table: "FeatureExamples",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureExamples_IsActive",
                table: "FeatureExamples",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureExamples_Name",
                table: "FeatureExamples",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureExamples_Status",
                table: "FeatureExamples",
                column: "Status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeatureExamples");
        }
    }
}
