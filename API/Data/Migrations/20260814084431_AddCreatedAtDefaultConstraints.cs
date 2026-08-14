using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatedAtDefaultConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.Sql("""
                ALTER TABLE Permissions
                ADD CONSTRAINT DF_Permissions_CreatedAt
                DEFAULT SYSUTCDATETIME() FOR CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Employees
                ADD CONSTRAINT DF_Employees_CreatedAt
                DEFAULT SYSUTCDATETIME() FOR CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Departments
                ADD CONSTRAINT DF_Departments_CreatedAt
                DEFAULT SYSUTCDATETIME() FOR CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Designations
                ADD CONSTRAINT DF_Designations_CreatedAt
                DEFAULT SYSUTCDATETIME() FOR CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Assets
                ADD CONSTRAINT DF_Assets_CreatedAt
                DEFAULT SYSUTCDATETIME() FOR CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Documents
                ADD CONSTRAINT DF_Documents_CreatedAt
                DEFAULT SYSUTCDATETIME() FOR CreatedAt;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                ALTER TABLE Permissions
                DROP CONSTRAINT DF_Permissions_CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Employees
                DROP CONSTRAINT DF_Employees_CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Departments
                DROP CONSTRAINT DF_Departments_CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Designations
                DROP CONSTRAINT DF_Designations_CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Assets
                DROP CONSTRAINT DF_Assets_CreatedAt;
                """);

            migrationBuilder.Sql("""
                ALTER TABLE Documents
                DROP CONSTRAINT DF_Documents_CreatedAt;
                """);
        }
    }
}
