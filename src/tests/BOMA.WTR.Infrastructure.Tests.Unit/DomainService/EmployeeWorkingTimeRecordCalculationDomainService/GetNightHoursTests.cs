using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class GetNightHoursTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _startWorkDate;
    private DateTime _endWorkDate;

    public GetNightHoursTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService();
    }

    private double Act() => _sut.GetNightHours(_startWorkDate, _endWorkDate);

    [Theory]
    [MemberData(nameof(DayHoursData))]
    public void DayHours_Should_ReturnZero(DateTime startDate, DateTime endDate, double workedHours)
    {
        // Arrange
        _startWorkDate = startDate;
        _endWorkDate = endDate;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(workedHours);
    }

    [Theory]
    [MemberData(nameof(SaturdayData))]
    public void HoursForSaturday_Should_ReturnZero(DateTime startDate, DateTime endDate)
    {
        // Arrange
        _startWorkDate = startDate;
        _endWorkDate = endDate;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(0);
    }

    public static IEnumerable<object[]> DayHoursData()
    {
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 6, 0, 0),
            new DateTime(2023, 11, 2, 14, 0, 0),
            0
        };
    }

    public static IEnumerable<object[]> SaturdayData()
    {
        yield return new object[]
        {
            new DateTime(2023, 11, 4, 0, 0, 0),
            new DateTime(2023, 11, 4, 23, 59, 59)
        };
    }
}