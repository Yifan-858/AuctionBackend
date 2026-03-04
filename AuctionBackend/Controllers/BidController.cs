using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionBackend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddBid([FromBody] BidCreateDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var bid = await _bidService.AddBid(dto.BidAmount, userId, dto.AuctionId);
                return Ok(bid);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBids()
        {
            var bids = await _bidService.GetAllBids();
            return Ok(bids);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBidsByUser(int userId)
        {
            var bids = await _bidService.GetBidByUser(userId);
            return Ok(bids);
        }

        [HttpGet("auction/{auctionId}")]
        public async Task<IActionResult> GetBidsByAuction(int auctionId)
        {
            var bids = await _bidService.GetBidByAuction(auctionId);
            return Ok(bids);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBid(int id)
        {
            try
            {
                var result = await _bidService.DeleteBid(id);
                if (result) return NoContent();
                return BadRequest("Failed to delete bid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
