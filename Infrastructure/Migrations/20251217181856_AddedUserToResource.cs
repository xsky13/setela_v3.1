using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SetelaServerV3._1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserToResource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SysUserId",
                table: "Resources",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Resources_SysUserId",
                table: "Resources",
                column: "SysUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Resources_SysUsers_SysUserId",
                table: "Resources",
                column: "SysUserId",
                principalTable: "SysUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Resources_SysUsers_SysUserId",
                table: "Resources");

            migrationBuilder.DropIndex(
                name: "IX_Resources_SysUserId",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "SysUserId",
                table: "Resources");
        }
    }
}
