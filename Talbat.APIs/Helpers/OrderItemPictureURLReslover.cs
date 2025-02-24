using AutoMapper;
using Talbat.APIs.DTOs;
using Talbat.Core.Entites.Order_Agg;

namespace Talbat.APIs.Helpers
{
    public class OrderItemPictureURLReslover : IValueResolver<OrderItem, OrderItemDTO, string>
    {
        private readonly IConfiguration _configuration;

        public OrderItemPictureURLReslover(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDTO destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
            {
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
