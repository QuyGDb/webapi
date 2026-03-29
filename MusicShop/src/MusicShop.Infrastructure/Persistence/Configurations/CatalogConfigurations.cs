using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicShop.Domain.Entities.Catalog;

namespace MusicShop.Infrastructure.Persistence.Configurations;

public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Genre)
            .HasMaxLength(100);

        builder.Property(x => x.Country)
            .HasMaxLength(100);
            
        // 1 Artist -> Many MasterReleases
        builder.HasMany(x => x.MasterReleases)
            .WithOne(x => x.Artist)
            .HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class LabelConfiguration : IEntityTypeConfiguration<Label>
{
    public void Configure(EntityTypeBuilder<Label> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        // 1 Label -> Many Releases
        builder.HasMany(x => x.Releases)
            .WithOne(x => x.Label)
            .HasForeignKey(x => x.LabelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class MasterReleaseConfiguration : IEntityTypeConfiguration<MasterRelease>
{
    public void Configure(EntityTypeBuilder<MasterRelease> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasIndex(x => x.Title); // Index for faster searching of master releases

        // 1 Master -> Many Releases
        builder.HasMany(x => x.Releases)
            .WithOne(x => x.Master)
            .HasForeignKey(x => x.MasterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ReleaseConfiguration : IEntityTypeConfiguration<Release>
{
    public void Configure(EntityTypeBuilder<Release> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Format)
            .HasConversion<string>(); // Store ReleaseFormat enum as text in the database

        // 1 Release -> Many Tracks
        builder.HasMany(x => x.Tracks)
            .WithOne(x => x.Release)
            .HasForeignKey(x => x.ReleaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(300);
    }
}
