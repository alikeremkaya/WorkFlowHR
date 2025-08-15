using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkFlowHR.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class mig9191 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "AppUsers",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "AppUsers");
        }
    }
}
