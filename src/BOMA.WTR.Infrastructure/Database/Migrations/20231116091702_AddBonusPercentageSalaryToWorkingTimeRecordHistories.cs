using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    public partial class AddBonusPercentageSalaryToWorkingTimeRecordHistories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalaryInformation_PercentageBonusSalary",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryInformation_PercentageBonusSalary",
                table: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
