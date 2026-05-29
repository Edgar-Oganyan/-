using CatalogApp.Models;
using CatalogApp.Repositories;

namespace CatalogApp.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Product>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Product?> GetByIdAsync(Guid id) => _repository.GetByIdAsync(id);

        public async Task<Product> CreateAsync(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Название товара обязательно.");
            if (product.Price < 0)
                throw new ArgumentException("Цена не может быть отрицательной.");
            product.UpdateAvailability();
            await _repository.AddAsync(product);
            return product;
        }

        public async Task<bool> UpdateAsync(Guid id, Product product)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return false;
            product.Id = id;
            product.UpdateAvailability();
            await _repository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return false;
            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> UpdateStockAsync(Guid id, int newStockCount)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product is null) return false;
            product.StockCount = newStockCount;
            product.UpdateAvailability();
            await _repository.UpdateAsync(product);
            return true;
        }
    }
}