using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuhaifSoftAPI.Migrations
{
    public partial class Afterremoveexpiredate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiryDate",
                table: "RefreshTokens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpiryDate",
                table: "RefreshTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
