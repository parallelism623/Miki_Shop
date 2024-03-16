using Ntier.DAL.DTO.Products;

namespace Ntier.BLL.Interfaces
{
    public interface IProductsService
    {
        public Task<ProductDTO> GetProductByIdAsync(string id);
        public Task AddProductAsync(ProductToAddDTO productToAdd);
        public Task UpdateProductAsync(ProductToAddDTO productToAdd);
        public Task<ProductsPagination> GetProductsAsync( ProductQueryParameters queryParameters );
        public Task DeleteProductsAsync(string[] productsId);
    }
}
