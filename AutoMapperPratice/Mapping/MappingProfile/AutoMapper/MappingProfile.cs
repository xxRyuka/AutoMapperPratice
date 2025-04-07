using System.Xml.Serialization;
using AutoMapper;

namespace AutoMapperPratice.Mapping.MappingProfile.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // P1 = Source , P2 = Destination
        CreateMap<Product, ProductListDto>()
            .ForMember(dest=>dest.ImageUrl,
                src => src.MapFrom(ent=>ent.Name + "" + ent.Price))
            .ReverseMap();

    }
}