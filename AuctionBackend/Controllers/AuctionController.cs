using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;
        private readonly IMapper _mapper;

        public AuctionController(IAuctionService auctionService, IMapper mapper)
        {
            _auctionService = auctionService;
            _mapper = mapper;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAuction([FromBody] AuctionCreateDto dto)
        {
            //var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
           
            try
            {
               var auctionDTO = await _auctionService.CreateAuction(dto.Title,dto.Description,dto.StartPrice,dto.StartDateUtc,dto.EndDateUtc,dto.UserId);
               return Ok(auctionDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
