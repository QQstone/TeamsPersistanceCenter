using Microsoft.EntityFrameworkCore.Migrations;

namespace TeamsPersistanceCenter.Models.Migrations
{
    public partial class DBSetUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AssignNumber",
                columns: table => new
                {
                    Num = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    isUsed = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignNumber", x => x.Num);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignNumber");
        }
    }
}
