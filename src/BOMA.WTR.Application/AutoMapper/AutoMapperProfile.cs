using AutoMapper;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;

namespace BOMA.WTR.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    private readonly IEmployeeWorkingTimeRecordCalculationDomainService _calculationDomainService;
    
    public AutoMapperProfile(IEmployeeWorkingTimeRecordCalculationDomainService calculationDomainService)
    {
        _calculationDomainService = calculationDomainService;
        
        CreateMap<Employee, EmployeeViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.RcpId, opt => opt.MapFrom(src => src.RcpId))
            .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.Salary.Amount))
            .ForMember(dest => dest.SalaryBonusPercentage, opt => opt.MapFrom(src => src.SalaryBonusPercentage.Value))
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
            .ForMember(dest => dest.DepartmentId, opt => opt.MapFrom(src => src.Department.Id))
            .ForMember(dest => dest.ShiftTypeId, opt => opt.MapFrom(src => src.JobInformation.ShiftType))
            .ForMember(dest => dest.ShiftTypeName, opt => opt.MapFrom(src => (int?)src.JobInformation.ShiftType))
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.JobInformation.Position.Name));
        
        CreateMap<WorkingTimeRecord, WorkingTimeRecordDetailsViewModel>()
            .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
            .ForMember(dest => dest.OccudedAt, opt => opt.MapFrom(src => src.OccuredAt))
            .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.MissingRecordEventType, opt => opt.MapFrom(src => src.MissingRecordEventType));

        CreateMap<EmployeeSalaryViewModel, EmployeeSalaryAggregatedHistory>();
        CreateMap<WorkingTimeRecordAggregatedHistory, WorkingTimeRecordAggregatedViewModel>();

        CreateMap<EmployeeWorkingTimeRecordViewModel, EmployeeSalaryViewModel>()
            .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.Employee.BaseSalary))
            .ForMember(dest => dest.Base50PercentageSalary, opt => opt.MapFrom(src => src.Employee.BaseSalary * 1.5m))
            .ForMember(dest => dest.Base100PercentageSalary, opt => opt.MapFrom(src => src.Employee.BaseSalary * 2m))
            .ForMember(dest => dest.BaseSaturdaySalary, opt => opt.MapFrom(src => src.Employee.BaseSalary * 2m))

            .ForMember(dest => dest.GrossBaseSalary,
                opt => opt.MapFrom(src => CalculateGrossBaseSalary(src)))
            .ForMember(dest => dest.GrossBase50PercentageSalary,
                opt => opt.MapFrom(src => CalculateGrossBase50PercentageSalary(src)))
            .ForMember(dest => dest.GrossBase100PercentageSalary,
                opt => opt.MapFrom(src => CalculateGrossBase100PercentageSalary(src)))
            .ForMember(dest => dest.GrossBaseSaturdaySalary,
                opt => opt.MapFrom(src => CalculateGrossBaseSaturdaySalary(src)))

            .ForMember(dest => dest.BonusBaseSalary,
                opt => opt.MapFrom(src => CalculateBonusBaseSalary(src)))
            .ForMember(dest => dest.BonusBase50PercentageSalary,
                opt => opt.MapFrom(src => CalculateBonusBase50PercentageSalary(src)))
            .ForMember(dest => dest.BonusBase100PercentageSalary,
                opt => opt.MapFrom(src => CalculateBonusBase100PercentageSalary(src)))
            .ForMember(dest => dest.BonusBaseSaturdaySalary,
                opt => opt.MapFrom(src => CalculateBonusBaseSaturdaySalary(src)))
            
            .ForMember(dest => dest.GrossSumBaseSalary,
                opt => opt.MapFrom(src => CalculateGrossSumBaseSalary(src)))
            .ForMember(dest => dest.GrossSumBase50PercentageSalary,
                opt => opt.MapFrom(src => CalculateGrossSumBase50PercentageSalary(src)))
            .ForMember(dest => dest.GrossSumBase100PercentageSalary,
                opt => opt.MapFrom(src => CalculateGrossSumBase100PercentageSalary(src)))
            .ForMember(dest => dest.GrossSumBaseSaturdaySalary,
                opt => opt.MapFrom(src => CalculateGrossSumBaseSaturdaySalary(src)))
            
            .ForMember(dest => dest.BonusSumSalary,
                opt => opt.MapFrom(src => CalculateBonusSumSalary(src)))
            
            .ForMember(dest => dest.NightBaseSalary,
                opt => opt.MapFrom(src => CalculateNightSumSalary(src)))
            
            .ForMember(dest => dest.NightWorkedHours,
                opt => opt.MapFrom(src => CalculateAllNightWorkedHours(src)))
            
            .ForMember(dest => dest.FinalSumSalary,
                opt => opt.MapFrom(src => CalculateFinalSumSalary(src)));
    }

    private static decimal CalculateGrossBaseSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.BaseNormativeHours);
    }

    private static decimal CalculateGrossBase50PercentageSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return src.Employee.BaseSalary * 1.5m * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.FiftyPercentageBonusHours);
    }

    private static decimal CalculateGrossBase100PercentageSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return src.Employee.BaseSalary * 2m * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.HundredPercentageBonusHours);
    }

    private static decimal CalculateGrossBaseSaturdaySalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return src.Employee.BaseSalary * 2m * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.SaturdayHours);
    }

    private static decimal CalculateBonusBaseSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBaseSalary(src) * src.Employee.SalaryBonusPercentage / 100;
    }

    private static decimal CalculateBonusBase50PercentageSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBase50PercentageSalary(src) * src.Employee.SalaryBonusPercentage / 100;
    }

    private static decimal CalculateBonusBase100PercentageSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBase100PercentageSalary(src) * src.Employee.SalaryBonusPercentage / 100;
    }

    private static decimal CalculateBonusBaseSaturdaySalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBaseSaturdaySalary(src) * src.Employee.SalaryBonusPercentage / 100;
    }

    private static decimal CalculateGrossSumBaseSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBaseSalary(src) + CalculateBonusBaseSalary(src);
    }

    private static decimal CalculateGrossSumBase50PercentageSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBase50PercentageSalary(src) + CalculateBonusBase50PercentageSalary(src);
    }

    private static decimal CalculateGrossSumBase100PercentageSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBase100PercentageSalary(src) + CalculateBonusBase100PercentageSalary(src);
    }

    private static decimal CalculateGrossSumBaseSaturdaySalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossBaseSaturdaySalary(src) + CalculateBonusBaseSaturdaySalary(src);
    }

    private static decimal CalculateBonusSumSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossSumBase50PercentageSalary(src) + CalculateGrossSumBase100PercentageSalary(src) + CalculateGrossSumBaseSaturdaySalary(src);
    }

    private decimal CalculateNightSumSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        var period = src.WorkingTimeRecordsAggregated.FirstOrDefault();
        if (period is null)
            return 0;
        
        var nightFactor = _calculationDomainService.GetNightFactorBonus(period.Date.Year, period.Date.Month);
        var nightWorkedHours = (decimal)CalculateAllNightWorkedHours(src);
        var nightSumSalary =  (decimal)nightFactor * nightWorkedHours;

        return nightSumSalary;
    }

    private decimal CalculateFinalSumSalary(EmployeeWorkingTimeRecordViewModel src)
    {
        return CalculateGrossSumBaseSalary(src) +
               CalculateGrossSumBase50PercentageSalary(src) +
               CalculateGrossSumBase100PercentageSalary(src) +
               CalculateGrossSumBaseSaturdaySalary(src) +
               CalculateNightSumSalary(src);
        // src.SalaryInformation.HolidaySalary +
        // src.SalaryInformation.SicknessSalary +
        // src.SalaryInformation.AdditionalSalary;
    }

    private static double CalculateAllNightWorkedHours(EmployeeWorkingTimeRecordViewModel src)
    {
        return src.WorkingTimeRecordsAggregated.Sum(x => x.NightHours);
    }
}