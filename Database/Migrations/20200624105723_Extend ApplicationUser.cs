using Microsoft.EntityFrameworkCore.Migrations;

namespace ObservrProgrammingTest.Data.Migrations
{
    public partial class ExtendApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewYorkBorough",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewYorkBorough",
                table: "AspNetUsers");
        }
    }
}
