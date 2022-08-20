using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    public partial class AddStartAndFinishOriginalWorkDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishNormalizedAt",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishOriginalAt",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartNormalizedAt",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartOriginalAt",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishNormalizedAt",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "FinishOriginalAt",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "StartNormalizedAt",
                table: "WorkingTimeRecordAggregatedHistories");

            migrationBuilder.DropColumn(
                name: "StartOriginalAt",
                table: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
