using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SetelaServerV3._1.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUpdateDateToAssignmentSubmission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "AssignmentSubmissions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "AssignmentSubmissions");
        }
    }
}
