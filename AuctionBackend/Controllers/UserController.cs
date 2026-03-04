using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IUserRepo userRepo, IMapper mapper, IUserService userService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignupDto signupDto)
        {
            try
            {
                var userName = await _userService.RegisterUser(signupDto);
                return Ok($"User {userName} is registered!");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

    }
}
