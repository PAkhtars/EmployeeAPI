using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class ActDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Section = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Offence = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Punishment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CognizableOrNon_Cognizable = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BailableOrNon_Bailable = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TrialCourt = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActName = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifeidBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActDetails", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActDetails");
        }
    }
}
