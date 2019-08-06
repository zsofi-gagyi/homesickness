using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HomesicknessVisualiser.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    Time = table.Column<DateTime>(nullable: false),
                    BpTemperature = table.Column<float>(nullable: false),
                    CsTemperature = table.Column<float>(nullable: false),
                    Index = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.Time);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Records");
        }
    }
}
