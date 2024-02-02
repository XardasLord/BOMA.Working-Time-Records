using BOMA.WTR.Application.Salary;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class GetSaturdayHoursTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _startWorkDate;
    private DateTime _endWorkDate;
    private double _workedHours;
    private readonly IOptions<SalaryConfiguration> _options = new OptionsWrapper<SalaryConfiguration>(new SalaryConfiguration { MinSalary = 4242 });

    public GetSaturdayHoursTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService(_options);
    }

    private double Act() => _sut.GetSaturdayHours(_startWorkDate, _endWorkDate, _workedHours);

    [Theory]
    [MemberData(nameof(TestWeekData))]
    public void WeekDay_Should_ReturnZero(DateTime date, double workedHours)
    {
        // Arrange
        _startWorkDate = date;
        _endWorkDate = date;
        _workedHours = workedHours;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(0);
    }

    [Theory]
    [MemberData(nameof(TestSaturdayData))]
    public void HoursForSaturday_Should_ReturnWorkedHours(DateTime date, double workedHours)
    {
        // Arrange
        _startWorkDate = date;
        _endWorkDate = date;
        _workedHours = workedHours;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().Be(workedHours);
    }

    public static IEnumerable<object[]> TestWeekData()
    {
        yield return new object[] { new DateTime(2022, 8, 1), 7 };
        yield return new object[] { new DateTime(2022, 8, 2), 7.5 };
        yield return new object[] { new DateTime(2022, 8, 3), 8 };
        yield return new object[] { new DateTime(2022, 8, 4), 8.5 };
        yield return new object[] { new DateTime(2022, 8, 5), 9 };
    }

    public static IEnumerable<object[]> TestSaturdayData()
    {
        yield return new object[] { new DateTime(2022, 7, 30), 7 };
        yield return new object[] { new DateTime(2022, 8, 6), 7.5 };
        yield return new object[] { new DateTime(2022, 8, 13), 8 };
        yield return new object[] { new DateTime(2022, 8, 20), 8.5 };
        yield return new object[] { new DateTime(2022, 8, 27), 9 };
    }
}