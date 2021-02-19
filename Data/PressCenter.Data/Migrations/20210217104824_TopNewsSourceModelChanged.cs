using Microsoft.EntityFrameworkCore.Migrations;

namespace PressCenter.Data.Migrations
{
    public partial class TopNewsSourceModelChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PageIsDynamic",
                table: "TopNewsSources",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageIsDynamic",
                table: "TopNewsSources");
        }
    }
}
