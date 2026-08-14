using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdatedAtTriggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                CREATE TRIGGER TR_Permissions_SetUpdatedAt
                ON Permissions
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE p
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM Permissions p
                    INNER JOIN inserted i
                        ON p.Id = i.Id;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER TR_Employees_SetUpdatedAt
                ON Employees
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE e
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM Employees e
                    INNER JOIN inserted i
                        ON e.Id = i.Id;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER TR_Departments_SetUpdatedAt
                ON Departments
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE d
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM Departments d
                    INNER JOIN inserted i
                        ON d.Id = i.Id;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER TR_Designations_SetUpdatedAt
                ON Designations
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE d
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM Designations d
                    INNER JOIN inserted i
                        ON d.Id = i.Id;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER TR_Assets_SetUpdatedAt
                ON Assets
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE a
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM Assets a
                    INNER JOIN inserted i
                        ON a.Id = i.Id;
                END;
                """);

            migrationBuilder.Sql("""
                CREATE TRIGGER TR_Documents_SetUpdatedAt
                ON Documents
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;

                    UPDATE d
                    SET UpdatedAt = SYSUTCDATETIME()
                    FROM Documents d
                    INNER JOIN inserted i
                        ON d.Id = i.Id;
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS TR_Permissions_SetUpdatedAt;
                """);

            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS TR_Employees_SetUpdatedAt;
                """);

            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS TR_Departments_SetUpdatedAt;
                """);

            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS TR_Designations_SetUpdatedAt;
                """);

            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS TR_Assets_SetUpdatedAt;
                """);

            migrationBuilder.Sql("""
                DROP TRIGGER IF EXISTS TR_Documents_SetUpdatedAt;
                """);
        }
    }
}
