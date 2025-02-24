using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talbat.APIs.DTOs;
using Talbat.APIs.Errors;
using Talbat.APIs.Helpers;
using Talbat.Core;
using Talbat.Core.Entites;
using Talbat.Core.Repositories;
using Talbat.Core.Specification;

namespace Talbat.APIs.Controllers
{
    
    public class ProductsController : ApiBaseController
    {
       
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
     
        public ProductsController(
            IMapper mapper,
           IUnitOfWork unitOfWork)
        {
       
            _mapper = mapper;
           _unitOfWork = unitOfWork;
           
        }
        //get all product
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery]ProSpecParams Params)
        {
            var Spec=new ProWithBrandAndTypeSpec(Params);
            var Products=await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(Spec);
            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products);
            var CountSpes = new ProWithFilterForCountAsyns(Params);
            var Count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpes);
            return Ok(new Pagination<ProductToReturnDTO>(Params.PageSize,Params.PageIndex,mappedProduct,Count));
        }

       // get by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var Spec = new ProWithBrandAndTypeSpec(id);
            var product=await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(Spec);
            if (product == null) return NotFound(new ApiResponse(404));
            var mappedProduct = _mapper.Map<Product,ProductToReturnDTO>(product);
            return Ok(mappedProduct);
        }
        //get all types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllProType()
        {
            var Types=await _unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(Types);
        }
        //get all brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllProBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(Brands);
        }
    }
}
