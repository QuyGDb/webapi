using MediatR;
using MusicShop.Application.DTOs.Catalog;
using MusicShop.Domain.Common;
using MusicShop.Domain.Entities.Catalog;
using MusicShop.Domain.Errors;
using MusicShop.Domain.Interfaces;

namespace MusicShop.Application.UseCases.Catalog.Labels.Commands.CreateLabel;

public sealed class CreateLabelCommandHandler(
    IRepository<Label> labelRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateLabelCommand, Result<LabelResponse>>
{
    public async Task<Result<LabelResponse>> Handle(
        CreateLabelCommand request, 
        CancellationToken cancellationToken)
    {
        // 1. Kiểm tra xem hãng đĩa đã tồn tại chưa (Dựa vào tên)
        var existingLabel = await labelRepository.FirstOrDefaultAsync(x => x.Name == request.Name);
        if (existingLabel != null)
        {
            return Result<LabelResponse>.Failure(LabelErrors.DuplicateName);
        }

        // 2. Tạo Entity mới từ Command
        var label = new Label
        {
            Name = request.Name,
            Country = request.Country,
            FoundedYear = request.FoundedYear,
            Website = request.Website
        };

        // 3. Thêm vào Repository
        labelRepository.Add(label);

        // 4. Lưu thay đổi vào Database
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 5. Trả về thông tin hãng đĩa đã tạo
        return Result<LabelResponse>.Success(new LabelResponse
        {
            Id = label.Id,
            Name = label.Name,
            Country = label.Country,
            FoundedYear = label.FoundedYear,
            Website = label.Website
        });
    }
}
