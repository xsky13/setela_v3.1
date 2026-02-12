using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SetelaServerV3._1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedClosedPropertyToExams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Closed",
                table: "Exams",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "Exams");
        }
    }
}
