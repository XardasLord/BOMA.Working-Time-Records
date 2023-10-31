using BOMA.WTR.Domain.SharedKernel;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class NormalizeDateTimeTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _date;
    private RecordEventType _recordEventType;

    public NormalizeDateTimeTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService();
    }

    private DateTime Act() => _sut.NormalizeDateTime(_recordEventType, _date);

    [Theory]
    [MemberData(nameof(FirstShiftEntryData))]
    public void FirstShiftEntry_Should_ReturnProperCalculatedEntryDateTime(DateTime entry, DateTime expectedEntry)
    {
        // Arrange
        _recordEventType = RecordEventType.Entry;
        _date = entry;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(expectedEntry);
    }

    [Theory]
    [MemberData(nameof(SecondShiftEntryData))]
    public void SecondShiftEntry_Should_ReturnProperCalculatedEntryDateTime(DateTime entry, DateTime expectedEntry)
    {
        // Arrange
        _recordEventType = RecordEventType.Entry;
        _date = entry;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(expectedEntry);
    }

    [Theory]
    [MemberData(nameof(ThirdShiftEntryData))]
    public void ThirdShiftEntry_Should_ReturnProperCalculatedEntryDateTime(DateTime entry, DateTime expectedEntry)
    {
        // Arrange
        _recordEventType = RecordEventType.Entry;
        _date = entry;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(expectedEntry);
    }

    [Theory]
    [MemberData(nameof(ThirdShiftExitData))]
    public void ThirdShiftEntry_Should_ReturnProperCalculatedExitDateTime(DateTime exit, DateTime expectedExit)
    {
        // Arrange
        _recordEventType = RecordEventType.Exit;
        _date = exit;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(expectedExit);
    }

    [Fact]
    public void EntryWithNoneSpecifiedEntryType_Should_ReturnTheSameEntryDateTime()
    {
        // Arrange
        _recordEventType = RecordEventType.None;
        _date = new DateTime(2022, 8, 1, 6, 45, 20);
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(_date);
    }

    [Fact]
    public void EntryWithInvalidEntryType_Should_ThrowsException()
    {
        // Arrange
        _recordEventType = (RecordEventType)999;
        _date = new DateTime(2022, 8, 1, 6, 45, 20);
        
        // Act
        var result = Record.Exception(() => Act());
        
        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<ArgumentOutOfRangeException>();
    }

    public static IEnumerable<object[]> FirstShiftEntryData()
    {
        yield return new object[] { 
            new DateTime(2022, 8, 1, 6, 0, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 5, 50, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 5, 40, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 5, 30, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 5, 20, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 5, 10, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 5, 0, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 4, 0, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 3, 0, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 2, 0, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 6, 5, 0),
            new DateTime(2022, 8, 1, 6, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 6, 6, 0),
            new DateTime(2022, 8, 1, 6, 30, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 6, 39, 0),
            new DateTime(2022, 8, 1, 6, 30, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 6, 40, 0),
            new DateTime(2022, 8, 1, 7, 0, 0)
        };
    }

    public static IEnumerable<object[]> SecondShiftEntryData()
    {
        yield return new object[] { 
            new DateTime(2022, 8, 1, 16, 0, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 15, 50, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 15, 40, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 15, 30, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 15, 20, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 15, 10, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 15, 0, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 14, 55, 0),
            new DateTime(2022, 8, 1, 15, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 16, 5, 0),
            new DateTime(2022, 8, 1, 16, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 16, 6, 0),
            new DateTime(2022, 8, 1, 16, 30, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 16, 39, 0),
            new DateTime(2022, 8, 1, 16, 30, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 16, 40, 0),
            new DateTime(2022, 8, 1, 17, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 17, 5, 0),
            new DateTime(2022, 8, 1, 17, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 17, 6, 0),
            new DateTime(2022, 8, 1, 17, 30, 0)
        };
    }

    public static IEnumerable<object[]> ThirdShiftEntryData()
    {
        yield return new object[] { 
            new DateTime(2022, 8, 1, 14, 0, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 13, 50, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 13, 40, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 13, 30, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 13, 20, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 13, 10, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 13, 0, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 12, 55, 0),
            new DateTime(2022, 8, 1, 13, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 14, 5, 0),
            new DateTime(2022, 8, 1, 14, 0, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 14, 6, 0),
            new DateTime(2022, 8, 1, 14, 30, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 14, 39, 0),
            new DateTime(2022, 8, 1, 14, 30, 0)
        };
        yield return new object[] { 
            new DateTime(2022, 8, 1, 14, 40, 0),
            new DateTime(2022, 8, 1, 15, 0, 0)
        };
    }

    public static IEnumerable<object[]> ThirdShiftExitData()
    {
        yield return new object[] { 
            new DateTime(2022, 8, 1, 23, 59, 0),
            new DateTime(2022, 8, 2, 0, 0, 0)
        };
    }
}