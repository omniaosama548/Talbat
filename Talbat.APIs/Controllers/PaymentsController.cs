using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talbat.APIs.DTOs;
using Talbat.APIs.Errors;
using Talbat.Core;
using Talbat.Core.Entites;

namespace Talbat.APIs.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService,
            IMapper mapper)
        {
            
            _paymentService = paymentService;
            _mapper = mapper;
        }
        //create or update endpoint
        [HttpPost]
        
        [ProducesResponseType(typeof(CustomerBasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CustomerBasketDTO>>CreateOrUpdatePayment(string basketId)
        {
            var customerBasket=await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (customerBasket == null) return BadRequest(new ApiResponse(400, "There is a problem in your Bassket"));
            var mappedBasket=_mapper.Map<CustomerBasket,CustomerBasketDTO>(customerBasket);
            return Ok(mappedBasket);    
        }
    }
}
