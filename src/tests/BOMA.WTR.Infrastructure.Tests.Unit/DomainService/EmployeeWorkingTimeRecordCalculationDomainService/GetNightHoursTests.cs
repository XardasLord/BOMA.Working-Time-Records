using BOMA.WTR.Domain.AggregateModels.Setting;
using BOMA.WTR.Domain.SharedKernel;
using BOMA.WTR.Tests.Base;
using FluentAssertions;
using Moq;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.EmployeeWorkingTimeRecordCalculationDomainService;

public class GetNightHoursTests : TestBase
{
    private readonly Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService _sut;
    private DateTime _startWorkDate;
    private DateTime _endWorkDate;
    private readonly Mock<IAggregateReadRepository<Setting>> _settingRepository = new();

    public GetNightHoursTests()
    {
        _sut = new Infrastructure.DomainService.EmployeeWorkingTimeRecordCalculationDomainService(_settingRepository.Object);
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
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 18, 0, 0),
            new DateTime(2023, 11, 3, 8, 0, 0),
            8
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
            8
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 23, 0, 0),
            new DateTime(2023, 11, 3, 6, 0, 0),
            7
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 5, 0, 0),
            7
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 23, 0, 0),
            new DateTime(2023, 11, 3, 5, 0, 0),
            6
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 20, 0, 0),
            new DateTime(2023, 11, 3, 2, 0, 0),
            4
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 22, 0, 0),
            new DateTime(2023, 11, 3, 2, 0, 0),
            4
        };
        // yield return new object[]
        // {
        //     new DateTime(2023, 11, 2, 22, 0, 0),
        //     new DateTime(2023, 11, 2, 23, 0, 0),
        //     1
        // };
        yield return new object[]
        {
            new DateTime(2023, 11, 2, 23, 0, 0),
            new DateTime(2023, 11, 3, 8, 0, 0),
            7
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 3, 2, 0, 0),
            new DateTime(2023, 11, 3, 8, 0, 0),
            4
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 3, 5, 0, 0),
            new DateTime(2023, 11, 3, 8, 0, 0),
            1
        };
        yield return new object[]
        {
            new DateTime(2023, 11, 3, 2, 0, 0),
            new DateTime(2023, 11, 3, 4, 0, 0),
            2
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