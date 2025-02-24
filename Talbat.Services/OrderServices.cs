using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core;
using Talbat.Core.Entites;
using Talbat.Core.Entites.Order_Agg;
using Talbat.Core.Repositories;
using Talbat.Core.Specification.OrderSpec;

namespace Talbat.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderServices(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //1)get basket from basket Repositore
            var Basket=await _basketRepository.GetBasketAsync(basketId);
            //2)get selected items at basket from ProductRepo
            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product=await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrderd = new ProductItemOrder(Product.Id,Product.Name,Product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrderd,  Product.Price, item.Quantity);
                    OrderItems.Add(orderItem);
                }
            }
            //3)calculate subtotal
            var subtotal=OrderItems.Sum(item=>item.Price*item.Quantity);
            //4)Get Delivery Method from DeliveryMethodRepo
            var deliveryMethod=await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
            //5)Create Order
            var spec = new OrderWithPaymentIntentSpecification(Basket.PaymentIntentId);
            var ExOrder=await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);
            if(ExOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(ExOrder);
                await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            }
            var Order = new Order(buyerEmail,ShippingAddress,deliveryMethod,OrderItems,subtotal,Basket.PaymentIntentId);
            //6)Add Order Locally
            await _unitOfWork.Repository<Order>().AddAsync(Order);
            //7)save order to database
            var Result=await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetAllDeliveryMethodAsync()
        {
           var DeliverMethods=await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return DeliverMethods;
        }

        public async Task<Order> GetOrderByIdForSpeficUserAsync(string BuyerEmail, int OrderId)
        {
            var Spec=new OrderSpecification(BuyerEmail, OrderId);
            var Order=await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(Spec);
            return Order;
        }

        public async Task<IReadOnlyList<Order>> GetOrderForSpeficUserAsync(string BuyerEmail)
        {
            var Spec=new OrderSpecification(BuyerEmail);
            var Orders=await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(Spec);
            return Orders;
        }
        
    }
}
