using Microsoft.EntityFrameworkCore.Migrations;

namespace CommunityLibrary.Migrations
{
    public partial class Identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReporterId",
                table: "Reports",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                column: "ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AspNetUsers_ReporterId",
                table: "Reports",
                column: "ReporterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AspNetUsers_ReporterId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
