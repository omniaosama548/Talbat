using AutoMapper;
using Talbat.APIs.DTOs;
using Talbat.Core.Entites;

namespace Talbat.APIs.Helpers
{
    public class ProductPictureUrlReslover : IValueResolver<Product, ProductToReturnDTO, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlReslover(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(Product source, ProductToReturnDTO destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.PictureUrl}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
