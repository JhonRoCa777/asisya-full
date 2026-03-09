
namespace Domain.Entities
{
    public record Supplier(
        Guid SupplierId,
        string CompanyName,
        string? ContactName,
        string? ContactTitle,
        string? Address,
        string? City,
        string? Region,
        string? PostalCode,
        string? Country,
        string? Phone,
        string? Fax,
        string? HomePage,

        DateTime CreatedAt,
        Guid CreatedBy,
        DateTime UpdatedAt,
        Guid UpdatedBy,
        DateTime? DeletedAt,
        Guid DeletedBy
    );

    public record SupplierDTO(
        Guid SupplierId,
        string CompanyName,
        string? ContactName,
        string? ContactTitle,
        string? Address,
        string? City,
        string? Region,
        string? PostalCode,
        string? Country,
        string? Phone,
        string? Fax,
        string? HomePage
    );

    public record SupplierRequestDTO(
        string CompanyName,
        string? ContactName,
        string? ContactTitle,
        string? Address,
        string? City,
        string? Region,
        string? PostalCode,
        string? Country,
        string? Phone,
        string? Fax,
        string? HomePage
    );
}
