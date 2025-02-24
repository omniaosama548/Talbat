using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entites.Order_Agg;

namespace Talbat.Services
{
    public interface IOrderServices
    {
        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrderForSpeficUserAsync(string BuyerEmail);
        Task<Order> GetOrderByIdForSpeficUserAsync(string BuyerEmail,int OrderId);
        Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync();

    }
}
