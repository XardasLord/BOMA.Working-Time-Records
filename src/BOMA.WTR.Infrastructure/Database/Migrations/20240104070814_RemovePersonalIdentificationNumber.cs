using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemovePersonalIdentificationNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalIdentityNumber",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonalIdentityNumber",
                table: "Employees",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);
        }
    }
}
