using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                var user = await _userService.Login(login);
                var token = _userService.GenerateToken(user);
                return Ok(new { token, user = new { user.Id, user.UserName, user.Email } });
            }
            catch (Exception ex)
            { return Unauthorized($"Invalid login: {ex}"); }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user == null) return NotFound();
                var dto = _mapper.Map<UserDto>(user);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] PasswordUpdateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var updated = await _userService.UpdateUserPassword(userId, dto.NewPassword);
                var userDto = _mapper.Map<UserDto>(updated);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (userId != id) return Forbid();
                await _userService.DeleteUser(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
