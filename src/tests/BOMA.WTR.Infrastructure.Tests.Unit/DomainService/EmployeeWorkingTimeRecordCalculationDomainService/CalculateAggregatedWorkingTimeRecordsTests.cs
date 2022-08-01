﻿using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.SharedKernel;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class CalculateAggregatedWorkingTimeRecords : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private IList<WorkingTimeRecord> _workingTimeRecords;

    public CalculateAggregatedWorkingTimeRecords()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService();
        _workingTimeRecords = new List<WorkingTimeRecord>();
    }

    private List<WorkingTimeRecordAggregatedViewModel> Act()
        => _sut.CalculateAggregatedWorkingTimeRecords(_workingTimeRecords).ToList();

    [Fact]
    public void ProvidedEmptyWorkingTimeRecords_Should_ReturnEmptyList()
    {
        // Arrange
        _workingTimeRecords = new List<WorkingTimeRecord>();
        
        // Act
        var result = Act();
        
        // Assert
        result.Count.Should().Be(0);
    }

    [Fact]
    public void ProvidedWrongDateOrder_Should_ReturnCorrectResult()
    {
        // Arrange
        var firstDay = new DateTime(2022, 7, 4);
        var secondDay = new DateTime(2022, 7, 5);
        _workingTimeRecords = new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 7, 5, 12, 0, 0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 7, 5, 20, 0, 0), 0),
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 7, 4, 8, 0, 0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 7, 4, 12, 0, 0), 0),
        };
        
        // Act
        var result = Act();
        
        // Assert
        result.Count.Should().Be(2);
        result.First().Date.Should().Be(firstDay);
        result.First().WorkedHoursRounded.Should().Be(4);
        
        result.Last().Date.Should().Be(secondDay);
        result.Last().WorkedHoursRounded.Should().Be(8);
    }

    [Theory]
    [MemberData(nameof(SingleDayEdgeCasesData))]
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
    [MemberData(nameof(TwoDaysEdgeCasesData))]
    public void ProvidedWorkingTimeRecordsForTwoDays_Should_ReturnProperCalculatedWorkedHours(
        List<WorkingTimeRecord> records, DateTime firstDay, DateTime secondDay, double expectedHoursForFirstDay, double expectedHoursForSecondDay)
    {
        // Arrange
        _workingTimeRecords = records;
        
        // Act
        var result = Act();

        // Assert
        result.Count.Should().Be(2);
        
        result.First().Date.Should().Be(firstDay);
        result.First().WorkedHoursRounded.Should().Be(expectedHoursForFirstDay);
        
        result.Last().Date.Should().Be(secondDay);
        result.Last().WorkedHoursRounded.Should().Be(expectedHoursForSecondDay);
    }
    
    [Theory]
    [MemberData(nameof(TwoDaysRealExamplesData))]
    public void ProvidedRealExamplesForTwoDays_Should_ReturnProperCalculatedWorkedHours(
        List<WorkingTimeRecord> records, 
        DateTime firstDay, DateTime secondDay, 
        double expectedHoursForFirstDay, double expectedHoursForSecondDay, 
        MissingRecordEventType? missingRecordEventTypeForFirstDay = null, MissingRecordEventType? missingRecordEventTypeForSecondDay = null)
    {
        // Arrange
        _workingTimeRecords = records;
        
        // Act
        var result = Act();

        // Assert
        result.First().Date.Should().Be(firstDay);
        result.First().WorkedHoursRounded.Should().Be(expectedHoursForFirstDay);
        
        if (missingRecordEventTypeForFirstDay.HasValue)
            result.First().MissingRecordEventType.Should().Be(missingRecordEventTypeForFirstDay);
        
        result.Last().Date.Should().Be(secondDay);
        result.Last().WorkedHoursRounded.Should().Be(expectedHoursForSecondDay);
        
        if (missingRecordEventTypeForSecondDay.HasValue)
            result.Last().MissingRecordEventType.Should().Be(missingRecordEventTypeForSecondDay);
    }

    [Theory]
    [MemberData(nameof(HoursCalculationEdgeCasesData))]
    public void ProvidedWorkingTimeRecordsForSingleDay_Should_ReturnProperCalculatedWorkedHoursForDifferentHourSections(
        List<WorkingTimeRecord> records, 
        double expectedTotalHours,
        double expectedBaseNormativeHours, 
        double expectedFiftyPercentageHours,
        double expectedHundredPercentageHours,
        double expectedSaturdayHours,
        double expectedNightHours)
    {
        // Arrange
        _workingTimeRecords = records;
        
        // Act
        var result = Act();
        
        // Assert
        result.Count.Should().Be(1);
        result.First().WorkedHoursRounded.Should().Be(expectedTotalHours);
        result.First().BaseNormativeHours.Should().Be(expectedBaseNormativeHours);
        result.First().FiftyPercentageBonusHours.Should().Be(expectedFiftyPercentageHours);
        result.First().HundredPercentageBonusHours.Should().Be(expectedHundredPercentageHours);
        result.First().SaturdayHours.Should().Be(expectedSaturdayHours);
        result.First().NightHours.Should().Be(expectedNightHours);
    }

    [Theory]
    [MemberData(nameof(HoursCalculationRealExamplesData))]
    public void ProvidedRealExamplesForSingleDay_Should_ReturnProperCalculatedWorkedHoursForDifferentHourSections(
        List<WorkingTimeRecord> records, 
        double expectedTotalHours,
        double expectedBaseNormativeHours, 
        double expectedFiftyPercentageHours,
        double expectedHundredPercentageHours,
        double expectedSaturdayHours,
        double expectedNightHours)
    {
        // Arrange
        _workingTimeRecords = records;
        
        // Act
        var result = Act();
        
        // Assert
        result.Count.Should().Be(1);
        result.First().WorkedHoursRounded.Should().Be(expectedTotalHours);
        result.First().BaseNormativeHours.Should().Be(expectedBaseNormativeHours);
        result.First().FiftyPercentageBonusHours.Should().Be(expectedFiftyPercentageHours);
        result.First().HundredPercentageBonusHours.Should().Be(expectedHundredPercentageHours);
        result.First().SaturdayHours.Should().Be(expectedSaturdayHours);
        result.First().NightHours.Should().Be(expectedNightHours);
    }

    [Theory]
    [MemberData(nameof(NightHoursCalculationEdgeCasesData))]
    public void ProvidedWorkingTimeRecordsForSingleDay_Should_ReturnProperCalculatedNightWorkedHoursForDifferentHourSections(
        List<WorkingTimeRecord> records, 
        double expectedTotalHours,
        double expectedBaseNormativeHours, 
        double expectedFiftyPercentageHours,
        double expectedHundredPercentageHours,
        double expectedSaturdayHours,
        double expectedNightHours)
    {
        // Arrange
        _workingTimeRecords = records;
        
        // Act
        var result = Act();
        
        // Assert
        result.Count.Should().Be(1);
        result.First().WorkedHoursRounded.Should().Be(expectedTotalHours);
        result.First().BaseNormativeHours.Should().Be(expectedBaseNormativeHours);
        result.First().FiftyPercentageBonusHours.Should().Be(expectedFiftyPercentageHours);
        result.First().HundredPercentageBonusHours.Should().Be(expectedHundredPercentageHours);
        result.First().SaturdayHours.Should().Be(expectedSaturdayHours);
        result.First().NightHours.Should().Be(expectedNightHours);
    }

    public static IEnumerable<object[]> SingleDayEdgeCasesData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0 ,0), 0)
        }, 8};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 5, 40 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0 ,0), 0)
        }, 8};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 5 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0 ,0), 0)
        }, 8};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 5, 39 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0 ,0), 0)
        }, 8.5};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 6 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0 ,0), 0)
        }, 7.5};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 13, 55,0), 0)
        }, 8};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 24,0), 0)
        }, 8};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 25,0), 0)
        }, 8.5};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 54,0), 0)
        }, 8.5};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 55,0), 0)
        }, 9};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 6 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 54,0), 0)
        }, 8};
    }

    public static IEnumerable<object[]> HoursCalculationEdgeCasesData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 8, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0 ,0), 0)
        }, 6, 6, 0, 0, 0, 0};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 8, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 16, 0 ,0), 0)
        }, 8, 8, 0, 0, 0, 0};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 8, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 16, 30 ,0), 0)
        }, 8.5, 8, 0.5, 0, 0, 0};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 8, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 18, 0 ,0), 0)
        }, 10, 8, 2, 0, 0, 0};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 8, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 18, 30 ,0), 0)
        }, 10.5, 8, 2, 0.5, 0, 0};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 8, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 22, 0 ,0), 0)
        }, 14, 8, 2, 4, 0, 0};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 1, 8, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 1, 14, 0 ,0), 0)
        }, 0, 0, 0, 0, 6, 0};
    }

    public static IEnumerable<object[]> HoursCalculationRealExamplesData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 6, 9, 15, 49 ,10), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 6, 10, 2, 1 ,10), 0)
        }, 10, 8, 2, 0, 0, 4};
    }

    public static IEnumerable<object[]> NightHoursCalculationEdgeCasesData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 22, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 5, 0 ,0), 0)
        }, 7, 7, 0, 0, 0, 7};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 20, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 4, 0 ,0), 0)
        }, 8, 8, 0, 0, 0, 6};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 20, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 8, 0 ,0), 0)
        }, 12, 8, 2, 2, 0, 8};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 4, 2, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 4, 0 ,0), 0)
        }, 2, 2, 0, 0, 0, 2};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 4, 2, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 10, 0 ,0), 0)
        }, 8, 8, 0, 0, 0, 4};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 4, 5, 52 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 16, 2 ,0), 0)
        }, 10, 8, 2, 0, 0, 0};
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 4, 5, 30 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 16, 0 ,0), 0)
        }, 10.5, 8, 2, 0.5, 0, 0.5};
    }
    
    public static IEnumerable<object[]> TwoDaysEdgeCasesData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
        {
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 4, 6, 0 ,0), 0),
            WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 14, 0 ,0), 0)
        }, 
            new DateTime(2022, 1, 3),
            new DateTime(2022, 1, 4),
            8, 
            8};
        yield return new object[] { new List<WorkingTimeRecord>
            {
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 5, 40,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 14, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 3, 18, 40,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 3, 22, 0,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 1, 4, 5, 39,0), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 1, 4, 14, 0,0), 0)
            }, 
            new DateTime(2022, 1, 3),
            new DateTime(2022, 1, 4),
            11, 
            8.5};
    }
    
    public static IEnumerable<object[]> TwoDaysRealExamplesData()
    {
        yield return new object[] { new List<WorkingTimeRecord>
            {
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 6, 7, 15, 52 ,50), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 6, 8, 2, 0 ,20), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 6, 9, 2, 1 ,30), 0),
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 6, 9, 15, 49 ,10), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 6, 10, 2, 1 ,10), 0)
            }, 
            new DateTime(2022, 6, 7),
            new DateTime(2022, 6, 9),
            10, 
            10};
        yield return new object[] { new List<WorkingTimeRecord>
            {
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 7, 2, 5, 45 ,10), 0),
                WorkingTimeRecord.Create(RecordEventType.Entry, new DateTime(2022, 7, 4, 5, 49 ,20), 0),
                WorkingTimeRecord.Create(RecordEventType.Exit, new DateTime(2022, 7, 4, 16, 1 ,30), 0)
            }, 
            new DateTime(2022, 7, 2),
            new DateTime(2022, 7, 4),
            0, 
            10,
            MissingRecordEventType.MissingExit
        };
    }
}