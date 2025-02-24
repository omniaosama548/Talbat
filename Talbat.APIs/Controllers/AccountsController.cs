using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talbat.APIs.DTOs;
using Talbat.APIs.Errors;
using Talbat.APIs.Extensions;
using Talbat.Core.Entites.Identity;
using Talbat.Core.Services;

namespace Talbat.APIs.Controllers
{
    
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenServices tokenServices,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>>Register(RegisterDTO model)
        {
            if (CheckEmailExist(model.Email).Result.Value)
                return BadRequest(new ApiResponse(400, "This Email Is Already Exist"));
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var Result=await _userManager.CreateAsync(User,model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(401));
            var ReturnedUser = new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenServices.CreateTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);
        }
        //login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>>Login(LoginDTO model)
        {
            var User=await _userManager.FindByEmailAsync(model.Email);
            if (User==null) return Unauthorized(new ApiResponse(401));
            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));
            return Ok(new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenServices.CreateTokenAsync(User, _userManager)
            });
        }
        //Get Current User
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email=User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);
        }
        //GetCurrentUserAddress
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
        {
            var user=await _userManager.FindUserWithAddressAsync(User);
            var mappedUser = _mapper.Map<Address, AddressDTO>(user.Address);
            return Ok(mappedUser);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateUserAddress")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO UpdatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user == null) return BadRequest(new ApiResponse(401));
            var address=_mapper.Map<AddressDTO,Address>(UpdatedAddress);
            address.Id = user.Address.Id;
            user.Address = address;
            var Result =await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(401));
            return Ok(UpdatedAddress);
        }
        //check email exist
        [HttpGet("CheckEmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
