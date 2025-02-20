using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TuhaifSoftAPI.Migrations
{
    public partial class addingmajortable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Major",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "Major_id",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    Major_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.Major_id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropColumn(
                name: "Major_id",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Major",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
