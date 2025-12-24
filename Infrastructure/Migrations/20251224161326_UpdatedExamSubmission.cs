using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SetelaServerV3._1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedExamSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Exam",
                table: "ExamSubmissions");

            migrationBuilder.DropColumn(
                name: "SysUser",
                table: "ExamSubmissions");

            migrationBuilder.CreateIndex(
                name: "IX_ExamSubmissions_SysUserId",
                table: "ExamSubmissions",
                column: "SysUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamSubmissions_SysUsers_SysUserId",
                table: "ExamSubmissions",
                column: "SysUserId",
                principalTable: "SysUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamSubmissions_SysUsers_SysUserId",
                table: "ExamSubmissions");

            migrationBuilder.DropIndex(
                name: "IX_ExamSubmissions_SysUserId",
                table: "ExamSubmissions");

            migrationBuilder.AddColumn<int>(
                name: "Exam",
                table: "ExamSubmissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SysUser",
                table: "ExamSubmissions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
