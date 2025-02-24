using System.ComponentModel.DataAnnotations;
using Talbat.Core.Entites.Order_Agg;

namespace Talbat.APIs.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }
        public AddressDTO ShippingAddress { get; set; }    
    }
}
