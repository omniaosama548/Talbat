﻿using Talbat.Core.Entites.Order_Agg;

namespace Talbat.APIs.DTOs
{
    public class OrderToReturnDTO
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } 
        public string Status { get; set; }
        public Address ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }//name
        public decimal DeliveryMethodCost { get; set; }//cost
        public ICollection<OrderItemDTO> Items { get; set; } = new HashSet<OrderItemDTO>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        
        public string PaymentIntentId { get; set; } 
    }

   
}
