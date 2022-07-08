using AutoMapper;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.GetRecords;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;

namespace BOMA.WTR.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Employee, EmployeeViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.RcpId, opt => opt.MapFrom(src => src.RcpId))
            .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.Salary.Amount))
            .ForMember(dest => dest.SalaryBonusPercentage, opt => opt.MapFrom(src => src.SalaryBonusPercentage.Value));
        
        CreateMap<WorkingTimeRecord, WorkingTimeRecordDetailsViewModel>()
            .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
            .ForMember(dest => dest.OccudedAt, opt => opt.MapFrom(src => src.OccuredAt))
            .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.GroupId));

        CreateMap<EmployeeWorkingTimeRecordViewModel, EmployeeSalaryViewModel>()
            .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.Employee.BaseSalary))
            .ForMember(dest => dest.Base50PercentageSalary, opt => opt.MapFrom(src => src.Employee.BaseSalary * 1.5m))
            .ForMember(dest => dest.Base100PercentageSalary, opt => opt.MapFrom(src => src.Employee.BaseSalary * 2m))
            .ForMember(dest => dest.BaseSaturdaySalary, opt => opt.MapFrom(src => src.Employee.BaseSalary * 2m))

            .ForMember(dest => dest.GrossBaseSalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.BaseNormativeHours)))
            .ForMember(dest => dest.GrossBase50PercentageSalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.FiftyPercentageBonusHours)))
            .ForMember(dest => dest.GrossBase100PercentageSalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.HundredPercentageBonusHours)))
            .ForMember(dest => dest.GrossBaseSaturdaySalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.SaturdayHours)))

            .ForMember(dest => dest.BonusBaseSalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.BaseNormativeHours) * src.Employee.SalaryBonusPercentage / 100))
            .ForMember(dest => dest.BonusBase50PercentageSalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.FiftyPercentageBonusHours) * src.Employee.SalaryBonusPercentage / 100))
            .ForMember(dest => dest.BonusBase100PercentageSalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.HundredPercentageBonusHours) * src.Employee.SalaryBonusPercentage / 100))
            .ForMember(dest => dest.BonusBaseSaturdaySalary,
                opt => opt.MapFrom(src => src.Employee.BaseSalary * (decimal)src.WorkingTimeRecordsAggregated.Sum(x => x.SaturdayHours) * src.Employee.SalaryBonusPercentage / 100));

        // TODO mapping for GrossSum and BonusSum
    }
}