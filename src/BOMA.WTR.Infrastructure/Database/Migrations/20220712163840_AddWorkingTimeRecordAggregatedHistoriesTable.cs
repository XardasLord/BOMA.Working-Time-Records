using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    public partial class AddWorkingTimeRecordAggregatedHistoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingTimeRecordAggregatedHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkedMinutes = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WorkedHoursRounded = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    BaseNormativeHours = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    FiftyPercentageBonusHours = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    HundredPercentageBonusHours = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SaturdayHours = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NightHours = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_BaseSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_Base50PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_Base100PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_BaseSaturdaySalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossBaseSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossBase50PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossBase100PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossBaseSaturdaySalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_BonusBaseSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_BonusBase50PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_BonusBase100PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_BonusBaseSaturdaySalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossSumBaseSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossSumBase50PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossSumBase100PercentageSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_GrossSumBaseSaturdaySalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_BonusSumSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    SalaryInformation_NightBaseSalary = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTimeRecordAggregatedHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingTimeRecordAggregatedHistories_Employees_UserId",
                        column: x => x.UserId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingTimeRecordAggregatedHistories_UserId",
                table: "WorkingTimeRecordAggregatedHistories",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
