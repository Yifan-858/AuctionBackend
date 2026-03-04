using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using AuctionBackend.Data.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuctionBackend.Core.Services
{
    public class UserService: IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IConfiguration configuration,IMapper mapper)
        {
            _userRepo = userRepo;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<string> RegisterUser([FromBody] SignupDto signupDto)
        {
            var isExisting = await _userRepo.GetUserByEmail(signupDto.Email);
            if(isExisting != null)
            {
                throw new Exception("Email is already registered.");
            }

            string passwordHash = signupDto.Password + "Hash";
            var user = await _userRepo.CreateUser(signupDto.UserName, signupDto.Email, passwordHash);

            return user.UserName;
        }
        public string GenerateToken(User user)
        {
            var key = _configuration["Jwt:Key"];
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Role,"User"));
                
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                     issuer: issuer,
                     audience: audience, 
                     claims: claims, 
                     expires: DateTime.UtcNow.AddMinutes(20), 
                     signingCredentials: signinCredentials);

     
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return tokenString;
        }

        public async Task<User> Login([FromBody] LoginDto login)
        {
            User? user = await _userRepo.GetUserByEmail(login.Email);

            if(user == null)
            {
                throw new Exception("Invalid login.");
            }

            var hashedPassword = login.Password + "Hash";

            if(user.PasswordHash != hashedPassword)
            {
                throw new Exception("Invalid login.");
            }

            return user;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _userRepo.GetUserById(userId);
        }

        public async Task<User> UpdateUserPassword(int userId, string newPassword)
        {
            var newPasswordHash = newPassword + "Hash";
            var user = await _userRepo.UpdateUserPassword(userId, newPasswordHash);

            return user;
        }

        public async Task DeleteUser(int userId)
        {
            var user = await _userRepo.GetUserById(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            await _userRepo.DeleteUser(user);
        }
    }
}
