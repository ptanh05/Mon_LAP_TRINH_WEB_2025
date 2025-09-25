using Thực_hành_model.Models;

namespace Thực_hành_model.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
        Task<List<Product>> SearchProductsAsync(string searchTerm);
    }
    
    public class ProductService : IProductService
    {
        private readonly List<Product> _products;
        private int _nextId = 1;

        public ProductService()
        {
            _products = new List<Product>
            {
                new Product { Id = _nextId++, Name = "Laptop Dell", Description = "Laptop Dell Inspiron 15", Price = 15000000, Quantity = 10 },
                new Product { Id = _nextId++, Name = "iPhone 15", Description = "Điện thoại iPhone 15 Pro Max", Price = 30000000, Quantity = 5 },
                new Product { Id = _nextId++, Name = "Samsung Galaxy S24", Description = "Điện thoại Samsung Galaxy S24 Ultra", Price = 25000000, Quantity = 8 }
            };
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(_products.Where(p => p.IsActive).ToList());
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == id && p.IsActive));
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            product.Id = _nextId++;
            product.CreatedAt = DateTime.Now;
            product.IsActive = true;
            _products.Add(product);
            return await Task.FromResult(product);
        }

        public async Task<Product?> UpdateProductAsync(int id, Product product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (existingProduct == null)
                return null;

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.Quantity = product.Quantity;
            existingProduct.ImageUrl = product.ImageUrl;
            
            return await Task.FromResult(existingProduct);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (product == null)
                return false;

            product.IsActive = false;
            return await Task.FromResult(true);
        }

        public async Task<List<Product>> SearchProductsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllProductsAsync();

            return await Task.FromResult(_products.Where(p => 
                p.IsActive && 
                (p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                 p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            ).ToList());
        }
    }
}
