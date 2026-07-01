using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppHost.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUsersSchemaToAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Identity",
                newName: "Users",
                newSchema: "Auth");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Auth",
                newName: "Users",
                newSchema: "Identity");
        }
    }
}
