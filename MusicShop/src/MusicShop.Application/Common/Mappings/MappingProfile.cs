using AutoMapper;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Entities.Catalog;

namespace MusicShop.Application.Common.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Genres
        CreateMap<Genre, GenreResponse>();

        // Labels
        CreateMap<Label, LabelResponse>();

        // Artists 
        CreateMap<Artist, ArtistResponse>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.ArtistGenres.Select(ag => ag.Genre)));

        // Releases
        CreateMap<Release, ReleaseResponse>()
            .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist.Name))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.ReleaseGenres.Select(rg => rg.Genre)));

        CreateMap<Release, ReleaseDetailResponse>()
            .IncludeBase<Release, ReleaseResponse>()
            .ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artist))
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.ReleaseGenres.Select(rg => rg.Genre)));

        // Release Versions
        CreateMap<ReleaseVersion, ReleaseVersionDto>()
            .ForMember(dest => dest.LabelName, opt => opt.MapFrom(src => src.Label.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src =>
                src.Products.SelectMany(p => p.Variants).OrderBy(v => v.Price).Select(v => (decimal?)v.Price).FirstOrDefault()))
            .ForMember(dest => dest.StockQty, opt => opt.MapFrom(src =>
                src.Products.SelectMany(p => p.Variants).Sum(v => v.StockQty)));

        // Tracks
        CreateMap<Track, TrackDto>();
    }
}
