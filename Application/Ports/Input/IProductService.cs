using Domain.Entities;
using Domain.Externals;

namespace Application.Ports.Input
{
    public interface IProductService
    {
        Task<Result<List<ProductDTO>>> FindAllAsync(
            string? nameFilter, int pageNumber, int pageSize, bool orderAsc
        );

        Task<Result<bool>> CreateByRowsAsync(int EntityRows, Guid ResponsableId);

        Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId);
    }
}
