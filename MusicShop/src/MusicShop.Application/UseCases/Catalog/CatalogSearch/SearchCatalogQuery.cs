using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.CatalogSearch;

public record SearchCatalogQuery(string Q) : IRequest<Result<CatalogSearchResult>>;
