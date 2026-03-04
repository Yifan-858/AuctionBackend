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
        [Authorize]
        public async Task<IActionResult> CreateAuction([FromBody] AuctionCreateDto dto)
        {
            try
            {
               var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
               var auctionDTO = await _auctionService.CreateAuction(dto.Title,dto.Description,dto.StartPrice,dto.StartDateUtc,dto.EndDateUtc,userId);
               return Ok(auctionDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuctions()
        {
            var auctions = await _auctionService.GetAllAuctions();
            return Ok(auctions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuctionById(int id)
        {
            try
            {
                var auction = await _auctionService.GetAuctionById(id);
                return Ok(auction);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAuctionsByUser(int userId)
        {
            var auctions = await _auctionService.GetAuctionsByUser(userId);
            return Ok(auctions);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetAuctionsByTitle([FromQuery] string title)
        {
            try
            {
                var auctions = await _auctionService.GetAuctionsByTitle(title);
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAuction(int id, [FromBody] AuctionCreateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var updated = await _auctionService.UpdateAuction(id, dto.Title, dto.Description, dto.StartPrice);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                await _auctionService.DeleteAuction(id, userId);
                return Ok($"Auction deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
