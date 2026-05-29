using System.Text.Json;
using System.Text.Json.Serialization;
using CatalogApp.Models;

namespace CatalogApp.Repositories
{
    public class JsonProductRepository : IProductRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        public JsonProductRepository(string filePath)
        {
            _filePath = filePath;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            if (!File.Exists(_filePath))
                return new List<Product>();
            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Product>>(json, _options) ?? new();
        }

        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var products = await GetAllAsync();
            return products.FirstOrDefault(p => p.Id == id);
        }

        public async Task AddAsync(Product product)
        {
            var products = await GetAllAsync();
            products.Add(product);
            await SaveAllAsync(products);
        }

        public async Task UpdateAsync(Product product)
        {
            var products = await GetAllAsync();
            var index = products.FindIndex(p => p.Id == product.Id);
            if (index >= 0)
                products[index] = product;
            await SaveAllAsync(products);
        }

        public async Task DeleteAsync(Guid id)
        {
            var products = await GetAllAsync();
            products.RemoveAll(p => p.Id == id);
            await SaveAllAsync(products);
        }

        private async Task SaveAllAsync(List<Product> products)
        {
            var dir = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
            var json = JsonSerializer.Serialize(products, _options);
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}