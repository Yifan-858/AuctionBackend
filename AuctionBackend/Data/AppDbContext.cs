using AuctionBackend.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionBackend.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        } 
        public DbSet<User> Users { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>()
                .Property(a => a.StartPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Auction>()
                .Property(a => a.SoldPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Bid>()
                .Property(b => b.BidAmount)
                .HasPrecision(18, 2);

            base.OnModelCreating(modelBuilder);
        }

    }
}
