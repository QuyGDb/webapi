using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicShop.Domain.Entities.Shop;

namespace MusicShop.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(x => x.Type)
            .HasConversion<string>(); // Map ProductType Enum to string

        builder.HasIndex(x => x.Name); // Index for faster product searching

        // 1 Product -> Many Variants
        builder.HasMany(x => x.Variants)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Sku)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Price)
            .HasPrecision(18, 2); // Currency data type

        builder.HasIndex(x => x.Sku).IsUnique(); // SKU must be unique
    }
}

public class ProductCollectionConfiguration : IEntityTypeConfiguration<ProductCollection>
{
    public void Configure(EntityTypeBuilder<ProductCollection> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Many-to-Many via ProductCollectionItem junction table
        builder.HasMany(x => x.Items)
            .WithOne(x => x.Collection)
            .HasForeignKey(x => x.CollectionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ProductCollectionItemConfiguration : IEntityTypeConfiguration<ProductCollectionItem>
{
    public void Configure(EntityTypeBuilder<ProductCollectionItem> builder)
    {
        // Declare Composite Key for junction table
        builder.HasKey(x => new { x.CollectionId, x.ProductId });

        builder.HasOne(x => x.Product)
            .WithMany(x => x.CollectionItems)
            .HasForeignKey(x => x.ProductId);
    }
}


