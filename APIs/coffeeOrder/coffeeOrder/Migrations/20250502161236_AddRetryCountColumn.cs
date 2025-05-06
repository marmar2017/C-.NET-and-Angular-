using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace coffeeOrder.Migrations
{
    /// <inheritdoc />
    public partial class AddRetryCountColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "Orders",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
