using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    public partial class AddAdditionalSalaryColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalaryInformation_AdditionalSalary",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryInformation_FinalSumSalary",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryInformation_HolidaySalary",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "SalaryInformation_NightWorkedHours",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryInformation_SicknessSalary",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryInformation_AdditionalSalary",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "SalaryInformation_FinalSumSalary",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "SalaryInformation_HolidaySalary",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "SalaryInformation_NightWorkedHours",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "SalaryInformation_SicknessSalary",
                table: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
