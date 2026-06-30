using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppHost.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdToIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "Identity",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                schema: "Identity",
                table: "Users",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Identity",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "NewId",
                schema: "Identity",
                table: "Users",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "Identity",
                table: "Users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "Identity",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "OldId",
                schema: "Identity",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Identity",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "OldId",
                schema: "Identity",
                table: "Users",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "Identity",
                table: "Users",
                column: "Id");
        }
    }
}
