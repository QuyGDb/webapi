using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicShop.Domain.Entities.Customer;
using MusicShop.Domain.Entities.System;

namespace MusicShop.Infrastructure.Persistence.Configurations;

public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
{
    public void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        builder.HasKey(x => x.Id);
    }
}

public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
{
    public void Configure(EntityTypeBuilder<Collection> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Condition).HasConversion<string>();
        builder.Property(x => x.PurchasePrice).HasPrecision(18, 2);
    }
}

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Comment).HasMaxLength(1000);
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(x => x.Email)
            .IsUnique(); // Email must be unique

        builder.Property(x => x.FullName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Role).HasConversion<string>();
    }
}

public class AdminActivityLogConfiguration : IEntityTypeConfiguration<AdminActivityLog>
{
    public void Configure(EntityTypeBuilder<AdminActivityLog> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Action).IsRequired().HasMaxLength(100);
        builder.Property(x => x.EntityType).IsRequired().HasMaxLength(100);
    }
}
