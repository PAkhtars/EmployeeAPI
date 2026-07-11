using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLegalCategoryIdToActMaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LegalCategoryId",
                table: "ActMasters",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActMasters_LegalCategoryId",
                table: "ActMasters",
                column: "LegalCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActMasters_LegalCategoryMasters_LegalCategoryId",
                table: "ActMasters",
                column: "LegalCategoryId",
                principalTable: "LegalCategoryMasters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActMasters_LegalCategoryMasters_LegalCategoryId",
                table: "ActMasters");

            migrationBuilder.DropIndex(
                name: "IX_ActMasters_LegalCategoryId",
                table: "ActMasters");

            migrationBuilder.DropColumn(
                name: "LegalCategoryId",
                table: "ActMasters");
        }
    }
}
