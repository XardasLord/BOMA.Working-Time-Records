using BOMA.WTR.Application.Salary;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class GetBaseNormativeHoursTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _startWorkDate;
    private DateTime _endWorkDate;
    private double _workedHours;
    private readonly IOptions<SalaryConfiguration> _options = new OptionsWrapper<SalaryConfiguration>(new SalaryConfiguration { MinSalary = 4242 });

    public GetBaseNormativeHoursTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService(_options);
    }

    private double Act() => _sut.GetBaseNormativeHours(_startWorkDate, _endWorkDate, _workedHours);

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
        yield return new object[] { new DateTime(2022, 8, 1), 7, 7 };
        yield return new object[] { new DateTime(2022, 8, 2), 7.5, 7.5 };
        yield return new object[] { new DateTime(2022, 8, 3), 8, 8 };
        yield return new object[] { new DateTime(2022, 8, 4), 8.5, 8 };
        yield return new object[] { new DateTime(2022, 8, 5), 9, 8 };
    }
}