using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BOMA.WTR.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToWorkingTimeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"                
                CREATE NONCLUSTERED INDEX IX_WTRAH_ActiveInMonth
                ON WorkingTimeRecordAggregatedHistories (Date)
                INCLUDE (UserId, DepartmentId, ShiftType)
                WHERE IsActive = 1;

                CREATE NONCLUSTERED INDEX IX_Employees_Name
                ON Employees (LastName, FirstName);

                CREATE NONCLUSTERED INDEX IX_WTRAH_Department_Shift
                ON WorkingTimeRecordAggregatedHistories (DepartmentId, ShiftType)
                INCLUDE (Date, IsActive, UserId);

                CREATE NONCLUSTERED INDEX IX_WTRAH_EffectiveQuery
                ON WorkingTimeRecordAggregatedHistories (UserId, Date, IsActive, DepartmentId)
                INCLUDE (
                    ShiftType,
                    SalaryInformation_FinalSumSalary, 
                    SalaryInformation_BonusSumSalary, 
                    WorkedHoursRounded,
                    WorkedMinutes
                );

                CREATE NONCLUSTERED INDEX IX_WorkingTimeRecords_Query
                ON WorkingTimeRecords (UserId, OccuredAt)
                INCLUDE (
                    DepartmentId,
                    EventType,
                    IsEditedManually,
                    MissingRecordEventType
                );

                CREATE NONCLUSTERED INDEX IX_Employees_ActiveDepartment
                ON Employees (IsActive, DepartmentId)
                INCLUDE (LastName, Id);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the indexes created in the Up method
            migrationBuilder.Sql(@"
                DROP INDEX IX_WTRAH_ActiveInMonth ON WorkingTimeRecordAggregatedHistories;
                DROP INDEX IX_Employees_Name ON WorkingTimeRecordAggregatedHistories;
                DROP INDEX IX_WTRAH_Department_Shift ON WorkingTimeRecordAggregatedHistories;
                DROP INDEX IX_WTRAH_EffectiveQuery ON WorkingTimeRecordAggregatedHistories;

                DROP INDEX IX_WorkingTimeRecords_Query ON WorkingTimeRecordAggregatedHistories;
                DROP INDEX IX_Employees_ActiveDepartment ON WorkingTimeRecordAggregatedHistories;
            ");
        }
    }
}
