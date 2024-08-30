using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RYT.Migrations
{
    public partial class update20243333 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Messages",
                newName: "ThreadId");

            //migrationBuilder.RenameColumn(
            //    name: "DeliverdOn",
            //    table: "Messages",
            //    newName: "DeliverOn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ThreadId",
                table: "Messages",
                newName: "MessageId");

            //migrationBuilder.RenameColumn(
            //    name: "DeliverOn",
            //    table: "Messages",
            //    newName: "DeliverdOn");
        }
    }
}
