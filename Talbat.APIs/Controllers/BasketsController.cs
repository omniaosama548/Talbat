using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talbat.APIs.DTOs;
using Talbat.APIs.Errors;
using Talbat.Core.Entites;
using Talbat.Core.Repositories;

namespace Talbat.APIs.Controllers
{
  
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository,
            IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }
        //get or recreate basket
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>>GetCustomerBasket(string BasketId)
        {
            var Basket=await _basketRepository.GetBasketAsync(BasketId);
            return Basket is null?new CustomerBasket(BasketId):Basket;
        }
        //update or create Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDTO>>UpdateBasket(CustomerBasketDTO Basket)
        {
            var MappedBasket=_mapper.Map<CustomerBasketDTO,CustomerBasket>(Basket); 
            var CreatedOrUpdatedBasket=await _basketRepository.UpdateBasketAsync(MappedBasket);
            if (CreatedOrUpdatedBasket is null) return BadRequest(new ApiResponse(400));
            return Ok(CreatedOrUpdatedBasket);  
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
