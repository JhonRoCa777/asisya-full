using Application.Ports.Output;
using AutoMapper;
using Bogus;
using Domain.Entities;
using Domain.Errors.Types;
using Domain.Externals;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions;

namespace Infrastructure.Repositories
{
    public class ProductRepository(
        AppDbContext Context,
        IMapper Mapper
    ) : IProductRepository
    {
        private readonly AppDbContext _Context = Context;
        private readonly IMapper _Mapper = Mapper;

        public async Task<Result<List<ProductDTO>>> FindAllAsync(
            string? nameFilter = null,
            int pageNumber = 1,
            int pageSize = 10,
            bool orderAsc = false
        )
        {
            var query = _Context.Products.AsQueryable();

            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(nameFilter))
            {
                query = query.Where(s => s.ProductName.Contains(nameFilter));
            }

            // Ordenamiento
            query = orderAsc
                ? query.OrderBy(s => s.CreatedAt)
                : query.OrderByDescending(s => s.CreatedAt);

            // Paginación
            var skip = (pageNumber - 1) * pageSize;
            var ModelList = await query
                .Include(p => p.Supplier)
                .Include(p => p.Category)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return Result<List<ProductDTO>>.Success(_Mapper.Map<List<ProductDTO>>(ModelList));
        }

        public async Task<Result<bool>> CreateByRowsAsync(int EntityRows, Guid ResponsableId)
        {
            var categories = await _Context.Categories.ToListAsync();
            var suppliers = await _Context.Suppliers.ToListAsync();
            var faker = new Faker<ProductModel>();

            var allProducts = new List<ProductModel>();

            // Traer nombres existentes para evitar violar unique constraint
            var existingNames = new HashSet<string>(
                await _Context.Products.Select(p => p.ProductName).ToListAsync()
            );

            // Distribuimos los registros entre categorías
            int categoriesCount = categories.Count;
            int rowsPerCategory = EntityRows / categoriesCount;
            int remainingRows = EntityRows % categoriesCount;

            foreach (var category in categories)
            {
                var categoryProducts = new List<ProductModel>();
                int targetCount = rowsPerCategory + (remainingRows-- > 0 ? 1 : 0);

                var productNames = new HashSet<string>(existingNames);

                // Faker para generar productos
                var productFaker = new Faker<ProductModel>()
                    .RuleFor(p => p.ProductId, f => Guid.NewGuid())
                    .RuleFor(p => p.ProductName, f =>
                    {
                        string name;
                        do
                        {
                            // Genera nombre único combinando Commerce.ProductName + sufijo aleatorio
                            name = f.Commerce.ProductName() + "-" + f.Random.AlphaNumeric(6);
                        } while (!productNames.Add(name));
                        return name;
                    })
                    .RuleFor(p => p.SupplierId, f => f.PickRandom(suppliers).SupplierId)
                    .RuleFor(p => p.CategoryId, category.CategoryId)
                    .RuleFor(p => p.UnitPrice, f => decimal.Parse(f.Commerce.Price(5, 500)))
                    .RuleFor(p => p.UnitsInStock, f => (short)f.Random.Int(0, 200))
                    .RuleFor(p => p.UnitsOnOrder, f => (short)f.Random.Int(0, 50))
                    .RuleFor(p => p.ReorderLevel, f => (short)f.Random.Int(5, 20))
                    .RuleFor(p => p.Discontinued, f => f.Random.Bool(0.1f))
                    .RuleFor(p => p.CreatedAt, DateTime.UtcNow)
                    .RuleFor(p => p.CreatedBy, ResponsableId)
                    .RuleFor(p => p.UpdatedAt, DateTime.UtcNow)
                    .RuleFor(p => p.UpdatedBy, ResponsableId)
                    .RuleFor(p => p.DeletedAt, (DateTime?)null)
                    .RuleFor(p => p.DeletedBy, Guid.Empty);

                // Generar productos
                while (categoryProducts.Count < targetCount)
                {
                    var product = productFaker.Generate();
                    categoryProducts.Add(product);
                }

                allProducts.AddRange(categoryProducts);
            }

            // Inserción masiva usando BulkExtensions optimizada para PostgreSQL
            await _Context.BulkInsertAsync(allProducts, options =>
            {
                options.BatchSize = 1000;
                options.SetOutputIdentity = false;
                options.UseTempDB = false;
                options.EnableStreaming = true; // optimiza COPY en PostgreSQL
            });

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteAsync(Guid EntityId, Guid ResponsableId)
        {
            var Result = await FindAsync(EntityId);
            if (!Result.IsSuccess)
                return Result<bool>.Failure(Result.Error);

            var Model = Result.Data;

            Model.DeletedAt = DateTime.UtcNow;
            Model.DeletedBy = ResponsableId;

            await _Context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        /************************************** CUSTOM **************************************/

        private async Task<Result<ProductModel>> FindAsync(Guid entityId)
        {
            var Model = await _Context.Products.FirstOrDefaultAsync(e => e.ProductId == entityId);

            if (Model == null)
                return Result<ProductModel>.Failure(new CategoryNotFoundError());

            if (Model.DeletedAt.HasValue)
                return Result<ProductModel>.Failure(new CategoryNotActiveError());

            return Result<ProductModel>.Success(Model);
        }
    }
}
