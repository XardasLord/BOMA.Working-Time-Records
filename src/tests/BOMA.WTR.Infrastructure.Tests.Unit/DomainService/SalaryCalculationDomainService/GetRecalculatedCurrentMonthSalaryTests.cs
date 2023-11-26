using AutoMapper;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Infrastructure.DomainService;
using FluentAssertions;
using Moq;
using Xunit;

namespace BOMA.WTR.Infrastructure.Tests.Unit.DomainService.SalaryCalculationDomainService;

public class GetRecalculatedCurrentMonthSalaryTests
{
    private readonly SalaryCalculationDomainServiceHandler _sut;

    private decimal _baseSalary;
    private decimal _percentageBonusSalary;
    private decimal _holidaySalary;
    private decimal _sicknessSalary;
    private decimal _additionalSalary;
    private List<WorkingTimeRecordAggregatedViewModel> _records;
    
    private EmployeeSalaryAggregatedHistory Act()
        => _sut.GetRecalculatedCurrentMonthSalary(_baseSalary, _percentageBonusSalary, _holidaySalary, _sicknessSalary, _additionalSalary, _records);

    public GetRecalculatedCurrentMonthSalaryTests()
    {
        var mapperMock = new Mock<IMapper>();
        var employeeRecordsCalculationMock = new Mock<IEmployeeWorkingTimeRecordCalculationDomainService>();

        _sut = new SalaryCalculationDomainServiceHandler(mapperMock.Object, employeeRecordsCalculationMock.Object);
        _baseSalary = 0;
        _percentageBonusSalary = 0;
        _holidaySalary = 0;
        _sicknessSalary = 0;
        _additionalSalary = 0;

        _records = new List<WorkingTimeRecordAggregatedViewModel>();
    }

    [Fact]
    public void CalculateProperSalary()
    {
        // Arrange
        _baseSalary = 20m;
        _percentageBonusSalary = 10m;
        _holidaySalary = 100m;
        _sicknessSalary = 200m;
        _additionalSalary = 300m;

        _records = new List<WorkingTimeRecordAggregatedViewModel>
        {
            new()
            {
                BaseNormativeHours = 168,
                FiftyPercentageBonusHours = 42,
                HundredPercentageBonusHours = 28,
                SaturdayHours = 50,
                NightHours = 60,
                WorkedHoursRounded = 168,
                WorkedMinutes = 168 * 60
            }
        };

        // Act
        var result = Act();

        // Assert
        result.BaseSalary.Should().Be(_baseSalary);
        result.PercentageBonusSalary.Should().Be(_percentageBonusSalary);
        result.HolidaySalary.Should().Be(_holidaySalary);
        result.SicknessSalary.Should().Be(_sicknessSalary);
        result.AdditionalSalary.Should().Be(_additionalSalary);

        result.Base50PercentageSalary.Should().Be(30m);
        result.Base100PercentageSalary.Should().Be(40m);
        result.BaseSaturdaySalary.Should().Be(40m);
        
        result.GrossBaseSalary.Should().Be(20 * 168);
        result.GrossBase50PercentageSalary.Should().Be(30 * 42);
        result.GrossBase100PercentageSalary.Should().Be(40 * 28);
        result.GrossBaseSaturdaySalary.Should().Be(40 * 50);
        
        result.BonusBaseSalary.Should().Be(result.GrossBaseSalary * _percentageBonusSalary / 100);
        result.BonusBase50PercentageSalary.Should().Be(result.GrossBase50PercentageSalary * _percentageBonusSalary / 100);
        result.BonusBase100PercentageSalary.Should().Be(result.GrossBase100PercentageSalary * _percentageBonusSalary / 100);
        result.BonusBaseSaturdaySalary.Should().Be(result.GrossBaseSaturdaySalary * _percentageBonusSalary / 100);
        
        result.GrossSumBaseSalary.Should().Be(result.GrossBaseSalary + result.BonusBaseSalary);
        result.GrossSumBase50PercentageSalary.Should().Be(result.GrossBase50PercentageSalary + result.BonusBase50PercentageSalary);
        result.GrossSumBase100PercentageSalary.Should().Be(result.GrossBase100PercentageSalary + result.BonusBase100PercentageSalary);
        result.GrossSumBaseSaturdaySalary.Should().Be(result.GrossBaseSaturdaySalary + result.BonusBaseSaturdaySalary);
        
        result.BonusSumSalary.Should().Be(result.GrossBase50PercentageSalary + result.GrossBase100PercentageSalary + result.GrossBaseSaturdaySalary);
        
        result.NightWorkedHours.Should().Be(60);
        result.NightBaseSalary.Should().Be(0); // TODO: Use mock to get night factor
        
        result.FinalSumSalary.Should().Be(
            result.GrossSumBaseSalary + 
            result.GrossSumBase50PercentageSalary + 
            result.GrossSumBase100PercentageSalary + 
            result.GrossSumBaseSaturdaySalary + 
            result.NightBaseSalary + 
            result.HolidaySalary + 
            result.SicknessSalary + 
            result.AdditionalSalary);
    }
}