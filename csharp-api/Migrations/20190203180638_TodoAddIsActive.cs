using Microsoft.EntityFrameworkCore.Migrations;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Migrations
{
    public partial class TodoAddIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Todos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "TodoId",
                keyValue: 1,
                column: "IsActive",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Todos");
        }
    }
}
