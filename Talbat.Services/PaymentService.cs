using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core;
using Talbat.Core.Entites;
using Talbat.Core.Entites.Order_Agg;
using Talbat.Core.Repositories;
using Product = Talbat.Core.Entites.Product;

namespace Talbat.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IBasketRepository basketRepository,
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string BasketId)
        {
            //stripe key
            StripeConfiguration.ApiKey = _configuration["StripeKeys:SecretKey"];
            //get basket
            var Basket=await _basketRepository.GetBasketAsync(BasketId);
            if (Basket == null) return null;
            var shippngPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var deliverMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DeliveryMethodId.Value);
                shippngPrice=deliverMethod.Cost;

            }
            if(Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    if(item.Price !=product.Price)
                        item.Price = product.Price;
                }
            }
            var subTotal=Basket.Items.Sum(item=>item.Price*item.Quantity);
            //create paymentIntent
            var Services = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(Basket.PaymentIntentId))//create
            {
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)shippngPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent=await Services.CreateAsync(Options);
                Basket.PaymentIntentId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)shippngPrice * 100
                };
                paymentIntent = await Services.UpdateAsync(Basket.PaymentIntentId,options);
                Basket.PaymentIntentId= paymentIntent.Id;
                Basket.ClientSecret= paymentIntent.ClientSecret;
            }
            await _basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }
    }
}
