using AutoMapper;
using WarehouseManagementSystem.Application.DTOs;
using WarehouseManagementSystem.Domain.Model;

namespace WarehouseManagementSystem.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductCreateDto, Product>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.Category, opt => opt.Ignore())
           .ForMember(dest => dest.Location, opt => opt.Ignore());


        CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Location, opt => opt.Ignore());

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.Location.Code))
            .ForMember(dest => dest.StockLimit, opt => opt.MapFrom(src => src.StockLimit))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));


        CreateMap<CategoryCreateDto, Category>();

        CreateMap<Category, CategoryDto>();


    }
}
