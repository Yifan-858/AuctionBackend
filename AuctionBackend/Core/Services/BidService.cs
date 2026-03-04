using AuctionBackend.Core.Interfaces;
using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using AuctionBackend.Data.Interfaces;
using AutoMapper;

namespace AuctionBackend.Core.Services
{
    public class BidService:IBidService
    {
        private readonly IBidRepo _bidRepo;
        private readonly IUserRepo _userRepo;
        private readonly IAuctionRepo _auctionRepo;
        private readonly IAuctionService _auctionService;
        private readonly IMapper _mapper;
        public BidService(IBidRepo bidRepo, IUserRepo userRepo,IAuctionRepo auctionRepo,IAuctionService auctionService,IMapper mapper)
        {
            _bidRepo = bidRepo;
            _userRepo = userRepo;
            _auctionRepo = auctionRepo;
            _auctionService = auctionService;
            _mapper = mapper;
        }

        public async Task<BidDto> AddBid(decimal bidAmount, int userId, int auctionId)
        {

            User user = await _userRepo.GetUserById(userId);

            if(user == null)
            {
                throw new Exception($"User with id: {userId} is not found");
            }

            var currentAuction = await _auctionService.GetAuctionById(auctionId);

            if(currentAuction == null)
            {
                throw new KeyNotFoundException("Auction not found");
            }
            
            if(currentAuction.UserId == userId)
            {
                throw new InvalidOperationException("You cannot bid on your own auction");
            }

            if(!currentAuction.IsOpen)
            {
                throw new InvalidOperationException("You cannot bid on a closed auction");
            }

            if (currentAuction.HighestBid.HasValue)
            {
                if (bidAmount <= currentAuction.HighestBid.Value)
                {
                    throw new InvalidOperationException("Bid must be higher than the current highest bid.");
                }
            }
            else
            {
                if (bidAmount < currentAuction.StartPrice)
                {
                    throw new InvalidOperationException("Bid must be at least the auction start price.");
                }
            }

            var createdAtUtc = DateTime.UtcNow;

            var bid = await _bidRepo.AddBid(bidAmount,createdAtUtc, userId, auctionId);

            if(bid == null)
            {
                throw new Exception($"Failed to add bid");
            }
            
            if (bid.User == null)
            {
                bid.User = user;
            }

            var bidDto = _mapper.Map<BidDto>(bid);
            return bidDto;
        }

        public async Task<List<BidDto>> GetAllBids()
        {
            var bids = await _bidRepo.GetAllBids();
            var bidDtos = _mapper.Map<List<BidDto>>(bids);
            return bidDtos;
        }

        public async Task<List<BidDto>> GetBidByUser(int userId)
        {
            User user = await _userRepo.GetUserById(userId);

            if(user == null)
            {
                throw new Exception($"User with id: {userId} is not found");
            }

            var bids = await _bidRepo.GetBidsByUser(userId);
            var bidDtos = _mapper.Map<List<BidDto>>(bids);

            return bidDtos;
        }

        public async Task<List<BidDto>> GetBidByAuction(int auctionId)
        {
            Auction auction = await _auctionRepo.GetAuctionById(auctionId);

            if(auction == null)
            {
                throw new Exception($"auction with id: {auctionId} is not found");
            }

            var bids = await _bidRepo.GetBidsByAuction(auctionId);
            var bidDtos = _mapper.Map<List<BidDto>>(bids);

            return bidDtos;
        }

        public async Task<bool> DeleteBid(int bidId)
        {
            Bid bid = await _bidRepo.GetSingleBid(bidId);
            if(bid == null)
            {
                throw new Exception($"Bid with id: {bidId} is not found");
            }

            return await _bidRepo.DeleteBid(bidId);
        }
    }
}
