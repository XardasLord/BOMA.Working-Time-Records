using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class GetFiftyPercentageBonusHoursTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _date;
    private double _workedHours;

    public GetFiftyPercentageBonusHoursTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService();
    }

    private double Act() => _sut.GetFiftyPercentageBonusHours(_date, _workedHours);

    [Fact]
    public void Saturday_Should_ReturnZero()
    {
        // Arrange
        _date = new DateTime(2022, 7, 30);
        _workedHours = 8;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(TestWeekData))]
    public void HoursForWeekDay_Should_ReturnProperCalculatedHours(DateTime date, double workedHours, double expectedHours)
    {
        // Arrange
        _date = date;
        _workedHours = workedHours;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(expectedHours);
    }

    public static IEnumerable<object[]> TestWeekData()
    {
        yield return new object[] { new DateTime(2022, 8, 1), 7, 0 };
        yield return new object[] { new DateTime(2022, 8, 2), 8, 0 };
        yield return new object[] { new DateTime(2022, 8, 3), 8.5, 0.5 };
        yield return new object[] { new DateTime(2022, 8, 4), 9, 1 };
        yield return new object[] { new DateTime(2022, 8, 5), 10, 2 };
        yield return new object[] { new DateTime(2022, 8, 5), 10.5, 2 };
    }
}