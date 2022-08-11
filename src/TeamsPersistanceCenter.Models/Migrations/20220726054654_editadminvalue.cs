using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsPersistanceCenter.Models.Migrations
{
    public partial class editadminvalue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "IsValid",
                table: "Administrator",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Administrator");
        }
    }
}
