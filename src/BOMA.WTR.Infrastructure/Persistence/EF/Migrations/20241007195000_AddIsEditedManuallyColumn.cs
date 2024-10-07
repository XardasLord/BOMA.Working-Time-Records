﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Persistence.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddIsEditedManuallyColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEditedManually",
                table: "WorkingTimeRecords",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEditedManually",
                table: "WorkingTimeRecordAggregatedHistories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEditedManually",
                table: "WorkingTimeRecords");

            migrationBuilder.DropColumn(
                name: "IsEditedManually",
                table: "WorkingTimeRecordAggregatedHistories");
        }
    }
}
