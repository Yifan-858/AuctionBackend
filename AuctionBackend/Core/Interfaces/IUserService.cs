using AuctionBackend.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace AuctionBackend.Core.Interfaces
{
    public interface IUserService
    {
        Task<string> RegisterUser([FromBody] SignupDto signupDto);
    }
}
