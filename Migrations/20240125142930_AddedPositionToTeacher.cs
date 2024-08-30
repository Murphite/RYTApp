using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RYT.Migrations
{
    public partial class AddedPositionToTeacher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Teachers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Teachers");
        }
    }
}
