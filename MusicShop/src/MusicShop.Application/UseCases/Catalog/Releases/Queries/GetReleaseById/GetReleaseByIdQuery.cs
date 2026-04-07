using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;

namespace MusicShop.Application.UseCases.Catalog.Releases.Queries.GetReleaseById;

public record GetReleaseByIdQuery(Guid Id) : IRequest<Result<ReleaseDetailResponse>>;
