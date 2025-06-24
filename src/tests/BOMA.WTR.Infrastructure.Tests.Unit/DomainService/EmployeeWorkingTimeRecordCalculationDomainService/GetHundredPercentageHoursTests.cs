using BOMA.WTR.Domain.AggregateModels.Setting;
using BOMA.WTR.Domain.SharedKernel;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class GetHundredPercentageBonusHoursTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _startWorkDate;
    private DateTime _endWorkDate;
    private double _workedHours;
    private readonly Mock<IAggregateReadRepository<Setting>> _settingRepository = new();

    public GetHundredPercentageBonusHoursTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService(_settingRepository.Object);
    }

    private double Act() => _sut.GetHundredPercentageBonusHours(_startWorkDate, _endWorkDate, _workedHours);

    [Fact]
    public void Saturday_Should_ReturnZero()
    {
        // Arrange
        _startWorkDate = new DateTime(2022, 7, 30);
        _endWorkDate = new DateTime(2022, 7, 30);
        _workedHours = 8;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(TestTwoDaysWorkData))]
    public void ExitsWorkAfterMidnight_Should_ReturnProperCalculatedHours(DateTime startDate, DateTime exitDate, double workedHours, double expectedHours)
    {
        // Arrange
        _startWorkDate = startDate;
        _endWorkDate = exitDate;
        _workedHours = workedHours;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(expectedHours);
    }

    [Theory]
    [MemberData(nameof(TestWeekData))]
    public void HoursForWeekDay_Should_ReturnProperCalculatedHours(DateTime date, double workedHours, double expectedHours)
    {
        // Arrange
        _startWorkDate = date;
        _endWorkDate = date;
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
        yield return new object[] { new DateTime(2022, 8, 3), 9, 0 };
        yield return new object[] { new DateTime(2022, 8, 4), 10, 0 };
        yield return new object[] { new DateTime(2022, 8, 5), 10.5, 0.5 };
        yield return new object[] { new DateTime(2022, 8, 5), 11, 1 };
        yield return new object[] { new DateTime(2022, 8, 5), 12, 2 };
        yield return new object[] { new DateTime(2022, 8, 5), 13, 3 };
        yield return new object[] { new DateTime(2022, 8, 5), 20, 10 };
    }

    public static IEnumerable<object[]> TestTwoDaysWorkData()
    {
        yield return new object[]
        {
            // Next day is saturday but exits work at 00:00
            new DateTime(2023, 10, 27, 14, 0, 0),
            new DateTime(2023, 10, 28, 0, 0, 0),
            9,
            0
        };
        yield return new object[]
        {
            // Next day is saturday but exits work at 01:00
            new DateTime(2023, 10, 27, 14, 0, 0),
            new DateTime(2023, 10, 28, 1, 0, 0),
            10,
            1
        };
        yield return new object[]
        {
            // Next day is saturday but exits work at 02:00
            new DateTime(2023, 10, 27, 14, 0, 0),
            new DateTime(2023, 10, 28, 2, 0, 0),
            11,
            2
        };
        yield return new object[]
        {
            // Next day is friday but exits work at 02:00
            new DateTime(2023, 10, 26, 14, 0, 0),
            new DateTime(2023, 10, 27, 2, 0, 0),
            11,
            1
        };
    }
}