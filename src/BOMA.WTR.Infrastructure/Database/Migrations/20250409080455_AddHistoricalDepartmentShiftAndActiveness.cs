using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoricalDepartmentShiftAndActiveness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte>(
                name: "ShiftType",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "tinyint",
                nullable: true);

            migrationBuilder.Sql(@"
UPDATE wtr
SET 
    wtr.DepartmentId = e.DepartmentId,
    wtr.IsActive = e.IsActive,
    wtr.ShiftType = e.ShiftType
FROM Boma.dbo.WorkingTimeRecordAggregatedHistories wtr
JOIN Employees e ON wtr.UserId = e.Id;");
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "ShiftType",
                table: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
