using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using AuctionBackend.Data.Interfaces;
using AuctionBackend.Data.Repos;
using AutoMapper;

namespace AuctionBackend.Core.Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepo _auctionRepo;
        private readonly IMapper _mapper;

        public AuctionService(IAuctionRepo auctionRepo,IMapper mapper)
        {
            _auctionRepo = auctionRepo;
            _mapper = mapper;
        }

        public async Task<AuctionCreateDto> CreateAuction(string title, string description, decimal startPrice, DateTime startDateUtc, DateTime endDateUtc,int userId)
        {
            var auction = await _auctionRepo.CreateAuction(title, description, startPrice, startDateUtc, endDateUtc, userId);
            if(auction == null)
            {
                throw new Exception("Failed to create auction.");
            }
             var auctionCreateDto = _mapper.Map<AuctionCreateDto>(auction);

            return auctionCreateDto;
        }

        public async Task<List<AuctionDto>> GetAllAuctions()
        {
            var auctions = await _auctionRepo.GetAllAuctions();
            var auctioDtos = _mapper.Map<List<AuctionDto>>(auctions);

            return auctioDtos;
        }

        public async Task<List<AuctionDto>> GetAuctionsByUser(int userId)
        {
            var auctions = await _auctionRepo.GetAuctionsByUser(userId);
            var auctioDtos = _mapper.Map<List<AuctionDto>>(auctions);

            return auctioDtos;
        }

        public async Task<AuctionDto> GetAuctionById(int auctionId)
        {
            var auction = await _auctionRepo.GetAuctionById(auctionId);

            var auctioDto = _mapper.Map<AuctionDto>(auction);
            return auctioDto;
        }

        public async Task<List<AuctionDto>> GetAuctionsByTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentNullException("Must provide valid title."); 
            }

            var auctions = await _auctionRepo.GetAuctionsByTitle(title);

            var auctionDtos = _mapper.Map<List<AuctionDto>>(auctions);
            return auctionDtos;
        }

        public async Task<AuctionDto> UpdateAuction(int auctionId, string? title, string? description, decimal? startPrice)
        {
            var auction = await _auctionRepo.UpdateAuction(auctionId, title, description, startPrice);
            var auctionDto = _mapper.Map<AuctionDto>(auction);
            return auctionDto;
        }

        public async Task DeleteAuction(int auctionId, int userId)
        {
            var currentAuction = await _auctionRepo.GetAuctionById(auctionId);

            if (currentAuction == null)
            {
               throw new KeyNotFoundException("Auction not found"); 
            }

            if (userId != currentAuction.UserId) 
            {
               throw new UnauthorizedAccessException("Unauthorized User: cannot delete other's auction.");
            }

            await _auctionRepo.DeleteAuction(auctionId);
        }
    }
}
