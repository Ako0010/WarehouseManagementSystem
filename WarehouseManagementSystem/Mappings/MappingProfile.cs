using AutoMapper;
using WarehouseManagementSystem.DTOs;
using WarehouseManagementSystem.Model;

namespace WarehouseManagementSystem.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ProductCreateDto, Product>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.Category, opt => opt.Ignore())
           .ForMember(dest => dest.Name, opt => opt.Ignore())
           .ForMember(dest => dest.Quantity, opt => opt.Ignore())
           .ForMember(dest => dest.Price, opt => opt.Ignore())
           .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

        CreateMap<ProductUpdateDto, Product>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Quantity, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
             .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

        CreateMap<SaleCreateDto, Sale>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
            .ForMember(dest => dest.SaleDate, opt => opt.Ignore());

        CreateMap<Sale, SaleDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

    }
}
