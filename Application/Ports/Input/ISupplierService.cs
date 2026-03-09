using Domain.Entities;
using Domain.Externals;

namespace Application.Ports.Input
{
    public interface ISupplierService
    {
        Task<Result<List<SupplierDTO>>> FindAllAsync();
    }
}
