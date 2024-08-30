using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RYT.Migrations
{
    public partial class SenderIdandRecieverIDaddedformessaging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeliverOn",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ThreadId",
                table: "Messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReadOn",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReceiverId",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.AddColumn<string>(
            //    name: "ReciverId",
            //    table: "Messages",
            //    type: "text",
            //    nullable: false,
            //    defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            //migrationBuilder.CreateIndex(
            //    name: "IX_Messages_ReciverId",
            //    table: "Messages",
            //    column: "ReciverId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Messages_AspNetUsers_ReciverId",
            //    table: "Messages",
            //    column: "ReciverId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Messages_AspNetUsers_ReciverId",
            //    table: "Messages");

            //migrationBuilder.DropIndex(
            //    name: "IX_Messages_ReciverId",
            //    table: "Messages");

            migrationBuilder.DropColumn(
                name: "DeliverOn",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ThreadId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReadOn",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            //migrationBuilder.DropColumn(
            //    name: "ReciverId",
            //    table: "Messages");

            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "Messages");
        }
    }
}
