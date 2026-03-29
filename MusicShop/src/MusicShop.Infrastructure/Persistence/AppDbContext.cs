using Microsoft.EntityFrameworkCore;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Entities.Shop;
using MusicShop.Domain.Entities.Orders;
using MusicShop.Domain.Entities.Customer;
using MusicShop.Domain.Entities.System;
using System.Reflection;

namespace MusicShop.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Music Catalog
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Label> Labels => Set<Label>();
    public DbSet<MasterRelease> MasterReleases => Set<MasterRelease>();
    public DbSet<Release> Releases => Set<Release>();
    public DbSet<Track> Tracks => Set<Track>();

    // Shop
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductCollection> ProductCollections => Set<ProductCollection>();
    public DbSet<ProductCollectionItem> ProductCollectionItems => Set<ProductCollectionItem>();
    public DbSet<PriceHistory> PriceHistories => Set<PriceHistory>();

    // Orders
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    // Customer
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<Review> Reviews => Set<Review>();

    // System
    public DbSet<User> Users => Set<User>();
    public DbSet<AdminActivityLog> AdminActivityLogs => Set<AdminActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Automatically scan and apply all IEntityTypeConfiguration in the Infrastructure project
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
