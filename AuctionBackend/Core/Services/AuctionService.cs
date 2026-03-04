using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
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
    }
}
