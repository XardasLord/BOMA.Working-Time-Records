using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    public partial class AddSalaryColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BaseSalaryAmount",
                table: "Employees",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "PLN");

            migrationBuilder.AddColumn<decimal>(
                name: "PercentageBonusSalary",
                table: "Employees",
                type: "money",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseSalaryAmount",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PercentageBonusSalary",
                table: "Employees");
        }
    }
}
