using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRHH.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeColumnsOnEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "employees",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "birth_date",
                table: "employees",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "created_at",
                table: "employees",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "now()");

            migrationBuilder.AddColumn<string>(
                name: "document_number",
                table: "employees",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "employees",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "hire_date",
                table: "employees",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "employees",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "employees",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "position",
                table: "employees",
                type: "character varying(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "salary",
                table: "employees",
                type: "numeric(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateOnly>(
                name: "termination_date",
                table: "employees",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "updated_at",
                table: "employees",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_document_number",
                table: "employees",
                column: "document_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_employees_email",
                table: "employees",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_employees_document_number",
                table: "employees");

            migrationBuilder.DropIndex(
                name: "IX_employees_email",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "address",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "birth_date",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "document_number",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "email",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "hire_date",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "position",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "salary",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "termination_date",
                table: "employees");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "employees");
        }
    }
}
