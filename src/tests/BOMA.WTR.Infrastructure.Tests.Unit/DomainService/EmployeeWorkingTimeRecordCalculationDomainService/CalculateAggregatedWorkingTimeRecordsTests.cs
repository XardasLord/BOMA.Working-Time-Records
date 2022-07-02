using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class CalculateAggregatedWorkingTimeRecords : TestBase
{
    private Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private IList<WorkingTimeRecord> _workingTimeRecords;

    public CalculateAggregatedWorkingTimeRecords()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService();
        _workingTimeRecords = new List<WorkingTimeRecord>();
    }

    private IEnumerable<WorkingTimeRecordAggregatedViewModel> Act()
        => _sut.CalculateAggregatedWorkingTimeRecords(_workingTimeRecords);

    [Fact]
    public void ProvidedEmptyWorkingTimeRecords_Should_ReturnEmptyList()
    {
        // Arrange
        _workingTimeRecords = new List<WorkingTimeRecord>();
        
        // Act
        var result = Act();
        
        // Assert
        result.Count().Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(SingleDayData))]
    public void ProvidedWorkingTimeRecordsForSingleDay_Should_ReturnProperCalculatedWorkedHours(List<WorkingTimeRecord> records, double expectedHours)
    {
        // Arrange
        _workingTimeRecords = records;
        
        // Act
        var result = Act();
        
        // Assert
        result.Count().Should().Be(1);
        result.First().WorkedHoursRounded.Should().Be(expectedHours);
    }

    [Theory]
    [MemberData(nameof(TwoDaysData))]
    public void ProvidedWorkingTimeRecordsForTwoDays_Should_ReturnProperCalculatedWorkedHours(
        List<WorkingTimeRecord> records, DateTime firstDay, DateTime secondDay, double expectedHoursForFirstDay, double expectedHoursForSecondDay)
    {
        // Arrange
        _workingTimeRecords = records;
        
        // Act
        var result = Act();
        
        // Assert
        result.Count().Should().Be(2);
        
        result.First().Date.Should().Be(firstDay);
        result.First().WorkedHoursRounded.Should().Be(expectedHoursForFirstDay);
        
        result.Last().Date.Should().Be(secondDay);
        result.Last().WorkedHoursRounded.Should().Be(expectedHoursForSecondDay);
    }
    
    public static IEnumerable<object[]> SingleDayData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 0 ,0), 0)
        }, 6};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 14 ,0), 0)
        }, 6};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 15 ,0), 0)
        }, 6.5};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 44 ,0), 0)
        }, 6.5};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 45 ,0), 0)
        }, 7};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 19, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 21, 0 ,0), 0)
        }, 8};
    }
    
    public static IEnumerable<object[]> TwoDaysData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 0,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 2, 12, 0,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 2, 20, 0,0), 0)
        }, 
            new DateTime(2022, 1, 1),
            new DateTime(2022, 1, 2),
            6, 
            8};
        yield return new object[] { new List<WorkingTimeRecord>
            {
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 12, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 18, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 20, 40,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 22, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 2, 8, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 2, 20, 16,0), 0)
            }, 
            new DateTime(2022, 1, 1),
            new DateTime(2022, 1, 2),
            7.5, 
            12.5};
        yield return new object[] { new List<WorkingTimeRecord>
            {
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 18, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 2, 4, 10,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 2, 8, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 2, 12, 0,0), 0)
            }, 
            new DateTime(2022, 1, 1),
            new DateTime(2022, 1, 2),
            10, 
            4};
    }
}