using Microsoft.EntityFrameworkCore.Migrations;

namespace TournamentGenerator.Migrations
{
    public partial class addtablenumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "PhysicalTables",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "PhysicalTables");
        }
    }
}
