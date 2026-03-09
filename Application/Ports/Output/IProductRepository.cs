using Domain.Entities;
using Domain.Externals;

namespace Application.Ports.Output
{
    public interface IProductRepository
    {
        Task<Result<List<ProductDTO>>> FindAllAsync(
            string? nameFilter = null,
            int pageNumber = 1,
            int pageSize = 10,
            bool orderAsc = false
        );

        Task<Result<bool>> CreateByRowsAsync(int EntityRows, Guid ResponsableId);

        Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId);
    }
}
