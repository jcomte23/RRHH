using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RRHH.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateDbAndDepartmentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    location = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: true),
                    budget = table.Column<decimal>(type: "numeric(14,2)", precision: 14, scale: 2, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_departments_code",
                table: "departments",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_departments_name",
                table: "departments",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
