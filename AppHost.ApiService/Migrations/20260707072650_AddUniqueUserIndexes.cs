using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppHost.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueUserIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "UX_Users_Email",
                schema: "Auth",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Users_Username",
                schema: "Auth",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_Users_Email",
                schema: "Auth",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "UX_Users_Username",
                schema: "Auth",
                table: "Users");
        }
    }
}
