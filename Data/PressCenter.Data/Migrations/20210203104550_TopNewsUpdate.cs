using Microsoft.EntityFrameworkCore.Migrations;

namespace PressCenter.Data.Migrations
{
    public partial class TopNewsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopNews_Sources_SourceId",
                table: "TopNews");

            migrationBuilder.DropForeignKey(
                name: "FK_TopNews_TopNewsSources_TopNewsSourceId",
                table: "TopNews");

            migrationBuilder.DropIndex(
                name: "IX_TopNews_TopNewsSourceId",
                table: "TopNews");

            migrationBuilder.DropColumn(
                name: "TopNewsSourceId",
                table: "TopNews");

            migrationBuilder.AddForeignKey(
                name: "FK_TopNews_TopNewsSources_SourceId",
                table: "TopNews",
                column: "SourceId",
                principalTable: "TopNewsSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopNews_TopNewsSources_SourceId",
                table: "TopNews");

            migrationBuilder.AddColumn<int>(
                name: "TopNewsSourceId",
                table: "TopNews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopNews_TopNewsSourceId",
                table: "TopNews",
                column: "TopNewsSourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_TopNews_Sources_SourceId",
                table: "TopNews",
                column: "SourceId",
                principalTable: "Sources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TopNews_TopNewsSources_TopNewsSourceId",
                table: "TopNews",
                column: "TopNewsSourceId",
                principalTable: "TopNewsSources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
