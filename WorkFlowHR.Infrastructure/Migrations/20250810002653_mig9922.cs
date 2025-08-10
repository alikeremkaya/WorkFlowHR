using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowHR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig9922 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Advances");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Advances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
