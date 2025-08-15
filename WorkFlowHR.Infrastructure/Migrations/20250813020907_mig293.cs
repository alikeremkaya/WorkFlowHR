using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowHR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig293 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AppUsers_ManagerAppUserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_ManagerAppUserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ManagerAppUserId",
                table: "Expenses");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_AppUserId",
                table: "Expenses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AppUsers_AppUserId",
                table: "Expenses",
                column: "AppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AppUsers_AppUserId",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_AppUserId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Expenses");

            migrationBuilder.AddColumn<Guid>(
                name: "ManagerAppUserId",
                table: "Expenses",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ManagerAppUserId",
                table: "Expenses",
                column: "ManagerAppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AppUsers_ManagerAppUserId",
                table: "Expenses",
                column: "ManagerAppUserId",
                principalTable: "AppUsers",
                principalColumn: "Id");
        }
    }
}
