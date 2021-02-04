using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PressCenter.Data.Migrations
{
    public partial class TopNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopNewsSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopNewsSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TopNews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    RemoteId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TopNewsSourceId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopNews_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TopNews_TopNewsSources_TopNewsSourceId",
                        column: x => x.TopNewsSourceId,
                        principalTable: "TopNewsSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopNews_IsDeleted",
                table: "TopNews",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_TopNews_SourceId",
                table: "TopNews",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TopNews_TopNewsSourceId",
                table: "TopNews",
                column: "TopNewsSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_TopNewsSources_IsDeleted",
                table: "TopNewsSources",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopNews");

            migrationBuilder.DropTable(
                name: "TopNewsSources");
        }
    }
}
