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
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName,
                opt => opt.MapFrom(src => src.Name.FirstName))
            .ForMember(dest => dest.LastName,
                opt => opt.MapFrom(src => src.Name.LastName))
            .ForMember(dest => dest.RcpId,
                opt => opt.MapFrom(src => src.RcpId));
        
        CreateMap<WorkingTimeRecord, WorkingTimeRecordDetailsViewModel>()
            .ForMember(dest => dest.EventType,
                opt => opt.MapFrom(src => src.EventType))
            .ForMember(dest => dest.OccudedAt,
                opt => opt.MapFrom(src => src.OccuredAt))
            .ForMember(dest => dest.GroupId,
                opt => opt.MapFrom(src => src.GroupId));
    }
}