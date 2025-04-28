using AutoMapper;
using BOMA.WTR.Application.UseCases.Employees.Queries.GetAll;
using BOMA.WTR.Application.UseCases.WorkingTimeRecords.Queries.Models;
using BOMA.WTR.Domain.AggregateModels;
using BOMA.WTR.Domain.AggregateModels.Entities;
using BOMA.WTR.Domain.AggregateModels.Interfaces;
using BOMA.WTR.Domain.AggregateModels.ValueObjects;

namespace BOMA.WTR.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile(IEmployeeWorkingTimeRecordCalculationDomainService calculationDomainService)
    {
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
            .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.JobInformation.Position.Name))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        CreateMap<WorkingTimeRecord, WorkingTimeRecordDetailsViewModel>()
            .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType))
            .ForMember(dest => dest.OccudedAt, opt => opt.MapFrom(src => src.OccuredAt))
            .ForMember(dest => dest.GroupId, opt => opt.MapFrom(src => src.DepartmentId))
            .ForMember(dest => dest.MissingRecordEventType, opt => opt.MapFrom(src => src.MissingRecordEventType));

        CreateMap<EmployeeSalaryViewModel, EmployeeSalaryAggregatedHistory>()
            .ReverseMap();

        CreateMap<WorkingTimeRecordAggregatedHistory, WorkingTimeRecordAggregatedViewModel>()
            .ForMember(dest => dest.IsWeekendDay, opt => opt.MapFrom(src => src.Date.DayOfWeek == DayOfWeek.Saturday || src.Date.DayOfWeek == DayOfWeek.Sunday))
            .ForMember(dest => dest.WorkTimePeriodOriginal, opt => opt.MapFrom(src => new WorkTimePeriod(src.StartOriginalAt, src.FinishOriginalAt)))
            .ForMember(dest => dest.WorkTimePeriodNormalized, opt => opt.MapFrom(src => new WorkTimePeriod(src.StartNormalizedAt, src.FinishNormalizedAt)))
            .ForMember(dest => dest.DayWorkTimePeriodNormalized, opt => opt.MapFrom(src => calculationDomainService.GetDayWorkTimePeriod(src.StartNormalizedAt, src.FinishNormalizedAt)))
            .ForMember(dest => dest.NightWorkTimePeriodNormalized, opt => opt.MapFrom(src => calculationDomainService.GetNightWorkTimePeriod(src.StartNormalizedAt, src.FinishNormalizedAt)));

        CreateMap<EmployeeSalaryAggregatedHistory, EmployeeSalaryViewModel>();
    }
}