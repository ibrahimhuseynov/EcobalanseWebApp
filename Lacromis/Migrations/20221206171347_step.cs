using Microsoft.EntityFrameworkCore.Migrations;

namespace Lacromis.Migrations
{
    public partial class step : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descrition2",
                table: "garbages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descrition3",
                table: "garbages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descrition2",
                table: "garbages");

            migrationBuilder.DropColumn(
                name: "Descrition3",
                table: "garbages");
        }
    }
}
