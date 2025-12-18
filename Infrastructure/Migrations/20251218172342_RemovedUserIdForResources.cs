using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SetelaServerV3._1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUserIdForResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_SysUsers_OwnerId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_OwnerId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Resources");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_OwnerId",
                table: "Resources",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_SysUsers_OwnerId",
                table: "Resources",
                column: "OwnerId",
                principalTable: "SysUsers",
                principalColumn: "Id");
        }
    }
}
