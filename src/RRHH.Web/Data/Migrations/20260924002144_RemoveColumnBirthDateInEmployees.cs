using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRHH.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumnBirthDateInEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "birth_date",
                table: "employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "birth_date",
                table: "employees",
                type: "date",
                nullable: true);
        }
    }
}
