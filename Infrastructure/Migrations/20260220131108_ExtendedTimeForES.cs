using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SetelaServerV3._1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedTimeForES : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdminExtendedTime",
                table: "ExamSubmissions",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminExtendedTime",
                table: "ExamSubmissions");
        }
    }
}
