using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowHR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig99 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "LeaveTypes",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "LeaveTypes",
                newName: "Type");
        }
    }
}
