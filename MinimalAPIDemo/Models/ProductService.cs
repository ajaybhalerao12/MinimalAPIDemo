
using Microsoft.EntityFrameworkCore;

namespace MinimalAPIDemo.Models
{
    public class ProductService : IProductsService
    {
        private readonly ApplicationDBContext _context;

        public ProductService(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
           return await _context.Products.FindAsync(id);
            //if (product == null)
            //    return null;
            //return product;
        }
       

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;

        }
        public async Task<Product> UpdateProductAsync(int id, Product product)
        {
            var productToUpdate = await _context.Products.FindAsync(id);
            if(productToUpdate == null)            
                return null;
            
            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description; 
            productToUpdate.Price = product.Price;

            await _context.SaveChangesAsync();

            return productToUpdate;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null) 
                return false;
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }        

       
    }
}
