using AutoMapper;
using Microsoft.Extensions.Configuration;
using ProductCatalog_BLL.DTOs;
using ProductCatalog_DAL.Models.Product;

namespace ProductCatalog_BLL.Helpers.PictureResolver
{
    public class PictureUrlResolver : IValueResolver<Products, ProductReturnDto, string>
    {

        private readonly IConfiguration _configuration;
        public PictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public string Resolve(Products source, ProductReturnDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
                return $"{_configuration["BaseImageProductUrl"]}/{source.PictureUrl}";
            return String.Empty;
        }
    }
}
