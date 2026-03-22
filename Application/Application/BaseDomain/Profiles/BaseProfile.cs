using AutoMapper;
using Domain.BaseDomain;

namespace Application.Application.BaseDomain.Profiles;

public class BaseProfile : Profile
{
    protected IMappingExpression<TSource, TDestination> IgnoreBaseProperties<TSource, TDestination>(
        IMappingExpression<TSource, TDestination> map)
        where TDestination : Base
    {
        return map
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Created, opt => opt.Ignore())
            .ForMember(dest => dest.Updated, opt => opt.Ignore());
    }
}