
namespace Domain.Entities
{
    public record Product(
        Guid ProductId,
        string ProductName,
        int SupplierId,
        int CategoryId,
        decimal UnitPrice,
        short UnitsInStock,
        short UnitsOnOrder,
        short ReorderLevel,
        bool Discontinued,

        DateTime CreatedAt,
        Guid CreatedBy,
        DateTime UpdatedAt,
        Guid UpdatedBy,
        DateTime? DeletedAt,
        Guid DeletedBy,

        Supplier? Supplier = null,
        Category? Category = null
    );

    public record ProductDTO(
        Guid ProductId,
        string ProductName,
        int SupplierId,
        int CategoryId,
        decimal UnitPrice,
        short UnitsInStock,
        short UnitsOnOrder,
        Supplier? Supplier = null,
        Category? Category = null
    );
}
