using System;

namespace CatalogApp.Models
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public int StockCount { get; set; }
        public ProductStatus Availability { get; set; } = ProductStatus.InStock;

        public void UpdateAvailability()
        {
            Availability = StockCount > 0 ? ProductStatus.InStock : ProductStatus.OutOfStock;
        }
    }
}