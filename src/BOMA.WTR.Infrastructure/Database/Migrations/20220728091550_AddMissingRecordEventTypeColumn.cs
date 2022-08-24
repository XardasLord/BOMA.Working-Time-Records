using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    public partial class AddMissingRecordEventTypeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "MissingRecordEventType",
                table: "WorkingTimeRecords",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "MissingRecordEventType",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MissingRecordEventType",
                table: "WorkingTimeRecords");

            migrationBuilder.DropColumn(
                name: "MissingRecordEventType",
                table: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
