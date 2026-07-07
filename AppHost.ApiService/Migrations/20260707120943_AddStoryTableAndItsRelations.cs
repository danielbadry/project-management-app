using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppHost.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class AddStoryTableAndItsRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stories",
                schema: "ProjectManagement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    AssignedId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stories_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "ProjectManagement",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stories_Users_AssignedId",
                        column: x => x.AssignedId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stories_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Auth",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stories_AssignedId",
                schema: "ProjectManagement",
                table: "Stories",
                column: "AssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_OwnerId",
                schema: "ProjectManagement",
                table: "Stories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stories_ProjectId",
                schema: "ProjectManagement",
                table: "Stories",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stories",
                schema: "ProjectManagement");
        }
    }
}
