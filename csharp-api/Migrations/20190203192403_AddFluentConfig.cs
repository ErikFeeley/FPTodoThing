using Microsoft.EntityFrameworkCore.Migrations;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Migrations
{
    public partial class AddFluentConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Todos",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "TodoId",
                keyValue: 1,
                column: "Description",
                value: "dummy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Todos",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 1000);

            migrationBuilder.UpdateData(
                table: "Todos",
                keyColumn: "TodoId",
                keyValue: 1,
                column: "Description",
                value: "Dummy");
        }
    }
}
