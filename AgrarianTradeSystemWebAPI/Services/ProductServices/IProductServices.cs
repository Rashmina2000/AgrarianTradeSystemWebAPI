using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgrarianTradeSystemWebAPI.Services.ProductServices
{
	public interface IProductServices
	{
		Task<List<Product>> GetAllProduct();
		Task<PagedResult<Product>> GetAllProductsPage(int pageNumber, int pageSize);
		Task<Product?> GetSingleProduct(int id);
		Task<List<Product>> AddProduct(Product product);
		Task<List<Product>?> UpdateProduct(int id, [FromForm] Product request, String newFileUrl);
		Task<List<Product>?> DeleteProduct(int id);
		Task<List<Product>> GetAllProductsSortedByPriceAsync(bool ascending = true);
		Task<List<ProductCardDto>> GetAllProductsWithFarmerDetails();
		Task<ProductListDto?> GetSingleProductDto(int id);
		Task<List<ProductCardDto>> GetAllProductsSortedByPrice(bool ascending = true);
		Task<List<Product>?> UpdateProductDetails(int id, [FromForm] Product request);
		Task<List<Product>?> UpdateProductImage(int id, String newFileUrl);
		Task<List<Product>> GetAllProductsByFarmer(string farmerId);
		Task<List<Product>> SearchAsync(string searchTerm);
		Task<PagedResult<Product>> GetAllProductsByFarmerPage(string farmerId, int pageNumber, int pageSize);


	}
}
