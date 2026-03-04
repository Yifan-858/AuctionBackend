using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
    }
}
