using Domain.Entities;
using Domain.Externals;

namespace Application.Ports.Output
{
    public interface ISupplierRepository
    {
        Task<Result<List<SupplierDTO>>> FindAllAsync();
    }
}
