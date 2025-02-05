using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddMinSalaryCompensationColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalaryInformation_MinSalaryCompensationAmount",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SalaryInformation_MinSalaryCompensationFactor",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "decimal(10,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryInformation_MinSalaryCompensationAmount",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "SalaryInformation_MinSalaryCompensationFactor",
                table: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
