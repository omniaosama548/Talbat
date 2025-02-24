using AutoMapper;
using Talbat.APIs.DTOs;
using Talbat.Core.Entites;
using Talbat.Core.Entites.Identity;
using Talbat.Core.Entites.Order_Agg;
using IdentityAddress = Talbat.Core.Entites.Identity.Address;
using OrderAddress = Talbat.Core.Entites.Order_Agg.Address;

namespace Talbat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.ProductType, O => O.MapFrom(S => S.ProductType.Name))
                .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlReslover>());

            CreateMap<IdentityAddress, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, OrderAddress>();

            CreateMap<CustomerBasketDTO,CustomerBasket>().ReverseMap();

            CreateMap<BasketItemDTO, BasketItem>().ReverseMap();

            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom < OrderItemPictureURLReslover>());
        }
    }
}
