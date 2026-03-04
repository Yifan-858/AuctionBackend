using AuctionBackend.Data.DTO;
using AuctionBackend.Data.Entities;
using AutoMapper;

namespace AuctionBackend.Data.Profiles
{
    public class AuctionProfile:Profile
    {
       public AuctionProfile()
        {
            CreateMap<Auction, AuctionCreateDto>();

            CreateMap<Auction, AuctionOpenDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(org => org.User.UserName))
                .ForMember(dest => dest.HighestBid, opt => opt.MapFrom(src => src.Bids != null && src.Bids.Any()
                    ? src.Bids.Max(b => b.BidAmount)
                    : (decimal?)null))
                .ForMember(dest => dest.IsOpen, opt => opt.MapFrom(_=>true))
                .ForMember(dest => dest.IsOwner, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var currentUserId = (int)context.Items["CurrentUserId"];
                    return src.UserId == currentUserId;
                }))
                .ForMember(dest => dest.CanBid, opt => opt.MapFrom((src, dest, destMmber, context) =>
                {
                    var currentUserId = (int)context.Items["CurrentUserId"];
                    return src.IsActive && src.EndDateUtc > DateTime.UtcNow && src.UserId != currentUserId;
                }))
                .ForMember(dest => dest.Bids, opt => opt.MapFrom(src => src.Bids.Where(b => !b.IsDeleted).OrderByDescending(b => b.CreatedAtUtc)));

            CreateMap<Auction, AuctionCloseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(org => org.User.UserName))
                .ForMember(dest => dest.SoldPrice, opt => opt.MapFrom(src => src.Bids != null && src.Bids.Any()
                    ? src.Bids.Max(b => b.BidAmount)
                    : (decimal?)null))
                .ForMember(dest => dest.IsOpen, opt => opt.MapFrom(_=>false));

            
        }
    }
}
