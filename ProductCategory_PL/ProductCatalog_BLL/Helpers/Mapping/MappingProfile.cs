using AutoMapper;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_BLL.Helpers.PictureResolver;
using ProductCatalog_DAL.Models.Product;

namespace ProductCatalog_BLL.Helpers.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
			CreateMap<Products, ProductReturnDto>()
                 .ForMember(p => p.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                 .ForMember(p => p.CategoryName, o => o.MapFrom(s => s.ProductCategory.Name))
                 .ForMember(p => p.PictureUrl, o => o.MapFrom<PictureUrlResolver>());


            // Mapping from Products to ProductDto
            CreateMap<Products, ProductDto>()
                .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.duration))
                .ForMember(dest => dest.startDate, opt => opt.MapFrom(src => src.startDate))
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore()); // Handled separately

            // Mapping from ProductDto to Products
            CreateMap<ProductDto, Products>()
                .ForMember(dest => dest.PictureUrl, opt => opt.Ignore()) 
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore()) 
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore()); 






        }
    }
}
