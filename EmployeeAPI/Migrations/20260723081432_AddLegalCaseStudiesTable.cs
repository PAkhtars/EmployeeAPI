using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLegalCaseStudiesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegalCaseStudies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseTitleAndCitation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FactsOfTheCase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProceduralHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegalIssues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArgumentsOfTheParties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelevantLaw = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourtsAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Judgment_Holding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CriticalAnalysis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpactOfTheJudgment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conclusion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    References = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegalCaseStudies", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegalCaseStudies");
        }
    }
}
