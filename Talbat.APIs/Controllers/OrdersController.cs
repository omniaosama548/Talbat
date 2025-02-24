using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using StackExchange.Redis;
using System.Net;
using System.Security.Claims;
using Talbat.APIs.DTOs;
using Talbat.APIs.Errors;
using Talbat.Core.Entites.Order_Agg;
using Talbat.Services;
using Order = Talbat.Core.Entites.Order_Agg.Order;

namespace Talbat.APIs.Controllers
{
  
    public class OrdersController : ApiBaseController
    {
        private readonly IOrderServices _orderServices;
        private readonly IMapper _mapper;

        public OrdersController(IOrderServices orderServices,
            IMapper mapper)
        {
            _orderServices = orderServices;
            _mapper = mapper;
        }
        //Create Order EndPoint
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDTO>>CreateOrder(OrderDTO orderDTO)
        {
            var BuyerEmail=User.FindFirstValue(ClaimTypes.Email);
            var MappedShippingAddress=_mapper.Map<AddressDTO,Address>(orderDTO.ShippingAddress);
            var Order = await _orderServices.CreateOrderAsync(BuyerEmail, orderDTO.BasketId, orderDTO.DeliveryMethodId, MappedShippingAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "There is a problem with your order"));
            var mappedOrder = _mapper.Map<Order, OrderToReturnDTO>(Order);
            return Ok(mappedOrder);
        }
        //Get Order For Spefic User EndPoint
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders=await _orderServices.GetOrderForSpeficUserAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(400,"There is no order for this user"));
            var MappedOrders=_mapper.Map<IReadOnlyList<Order>,IReadOnlyList<OrderToReturnDTO>>(Orders);
            return Ok(MappedOrders);
        }
        //Get Order By Id For Spefic User EndPoint
        [HttpGet("{id}")] //baseurl/api/Orders/1  =>id as segmant not query string
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForSpeficUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order=await _orderServices.GetOrderByIdForSpeficUserAsync(BuyerEmail, id);
            if (order is null) return NotFound(new ApiResponse(400, $"There is no order with id={id}for this user"));
            var mappedOrder = _mapper.Map<Order, OrderToReturnDTO>(order);
            return Ok(mappedOrder);
        }
        //get delivery Method EndPoint
        [HttpGet("DeliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var DeliveryMethods=await _orderServices.GetAllDeliveryMethodAsync();
            return Ok(DeliveryMethods);
        }
    }
}
