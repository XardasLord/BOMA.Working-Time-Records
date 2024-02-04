using BOMA.WTR.Application.Salary;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class GetNightWorkTimePeriodTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _startWorkDate;
    private DateTime _endWorkDate;
    private readonly IOptions<SalaryConfiguration> _options = new OptionsWrapper<SalaryConfiguration>(new SalaryConfiguration { MinSalary = 4242 });

    public GetNightWorkTimePeriodTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService(_options);
    }

    private WorkTimePeriod? Act() => _sut.GetNightWorkTimePeriod(_startWorkDate, _endWorkDate);

    [Theory]
    [MemberData(nameof(TestData))]
    public void GivenData_Should_CalculateProperWorkTimePeriod(DateTime startDate, DateTime endDate, DateTime expectedStartDate, DateTime expectedEndDate)
    {
        // Arrange
        _startWorkDate = startDate;
        _endWorkDate = endDate;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().NotBeNull();
        result?.From.Should().Be(expectedStartDate);
        result?.To.Should().Be(expectedEndDate);
    }
    
    [Theory]
    [MemberData(nameof(DayWorkPeriodData))]
    public void GivenDayWorkTimePeriodData_Should_ReturnNull(DateTime startDate, DateTime endDate)
    {
        // Arrange
        _startWorkDate = startDate;
        _endWorkDate = endDate;
        
        // Act
        var result = Act();
        
        // Assert
        result.Should().BeNull();
    }

    public static IEnumerable<object[]> TestData()
    {
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 7, 0, 0),
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 21, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 21, 0, 0),
            new DateTime(2023, 11, 3, 7, 0, 0),
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 3, 0, 0, 0),
            new DateTime(2023, 11, 3, 8, 0, 0),
            new DateTime(2023, 11, 3, 0, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 3, 2, 0, 0),
            new DateTime(2023, 11, 3, 5, 0, 0),
            new DateTime(2023, 11, 3, 2, 0, 0),
            new DateTime(2023, 11, 3, 5, 0, 0),
        };
    }
    
    public static IEnumerable<object[]> DayWorkPeriodData()
    {
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 6, 0, 0),
            new DateTime(2023, 11, 2, 22, 0, 0),
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 10, 0, 0),
            new DateTime(2023, 11, 2, 18, 0, 0),
        };
    }
}