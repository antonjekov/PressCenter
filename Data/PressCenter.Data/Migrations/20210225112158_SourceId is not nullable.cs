using Microsoft.EntityFrameworkCore.Migrations;

namespace PressCenter.Data.Migrations
{
    public partial class SourceIdisnotnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "News",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SourceId",
                table: "News",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
