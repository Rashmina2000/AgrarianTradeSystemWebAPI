using AgrarianTradeSystemWebAPI.Dtos;
using AgrarianTradeSystemWebAPI.Models;
using AgrarianTradeSystemWebAPI.Services.ProductServices;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AgrarianTradeSystemWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IProductServices _productServices;
		private readonly IMapper _mapper;
		private readonly IFileServices _fileServices;
		private const string AzureContainerName = "products";
		public ProductController(IProductServices productServices, IMapper mapper, IFileServices fileServices)
		{
			_productServices = productServices;
			_mapper = mapper;
			_fileServices = fileServices;
		}
		
		//get all products
		[HttpGet]
		public async Task<ActionResult<List<Product>>> GetAllProduct()
		{

			return await _productServices.GetAllProduct();
		}
		//get all product list with pagination
		[HttpGet("product-list")]
		public async Task<ActionResult<PagedResult<Product>>> GetAllProductPage(int pageNumber = 1, int pageSize = 10)
		{
			if (pageNumber < 1) pageNumber = 1;
			if (pageSize < 1) pageSize = 10;

			try
			{
				var result = await _productServices.GetAllProductsPage(pageNumber, pageSize);

				if (result.Items == null || result.Items.Count == 0)
				{
					return NotFound(new { message = "No products found." });
				}

				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { message = "An error occurred while retrieving products.", details = ex.Message });
			}
		}

		//get product list by farmer ID
		[HttpGet("farmer/{farmerId}")]
		public async Task<ActionResult<List<Product>>> GetAllProduct(string farmerId)
		{
			var products = await _productServices.GetAllProductsByFarmer(farmerId);
			if (products == null || !products.Any())
			{
				return NotFound();
			}
			return products;
		}

		//get product list by farmer ID with pagination
		[HttpGet("farmer-product/{farmerId}")]
		public async Task<ActionResult<PagedResult<Product>>> GetAllProductPage(string farmerId, int pageNumber, int pageSize)
		{
			if (pageNumber < 1 || pageSize < 1)
			{
				return BadRequest("Page number and page size must be greater than 0.");
			}

			var pagedResult = await _productServices.GetAllProductsByFarmerPage(farmerId, pageNumber, pageSize);

			if (pagedResult.Items == null || !pagedResult.Items.Any())
			{
				return NotFound();
			}

			return Ok(pagedResult);
		}

		//get sorted products
		[HttpGet("sorted")]
		public async Task<ActionResult<List<Product>>> GetProductsSortedByPrice([FromQuery] string sortOrder = "asc")
		{
			if (sortOrder.ToLower() != "asc" && sortOrder.ToLower() != "desc")
			{
				return BadRequest("Invalid sort order. Use 'asc' or 'desc'.");
			}

			var products = await _productServices.GetAllProductsSortedByPriceAsync(sortOrder.ToLower() == "asc");
			return Ok(products);
		}

		//get all sorted list
		[HttpGet("sorted-products")]
		public async Task<ActionResult<List<ProductCardDto>>> GetProductsSortedByPriceList([FromQuery] string sortOrder = "asc")
		{
			if (sortOrder.ToLower() != "asc" && sortOrder.ToLower() != "desc")
			{
				return BadRequest("Invalid sort order. Use 'asc' or 'desc'.");
			}
			var products = await _productServices.GetAllProductsSortedByPrice(sortOrder.ToLower() == "asc");

			return Ok(products);
		}

		//get all products with farmer's details
		[HttpGet("all-details")]
		public async Task<ActionResult<List<ProductCardDto>>> GetAllProductsWithFarmerDetails()
		{
			var productListDtos = await _productServices.GetAllProductsWithFarmerDetails();
			return Ok(productListDtos);
		}



		//get product details by id
		[HttpGet("{id}")]
		public async Task<ActionResult<List<Product>>> GetSingleProduct(int id)
		{
			var result = await _productServices.GetSingleProduct(id);
			if (result is null)
				return NotFound("product is not found");
			return Ok(result);
		}


		//get product and farmer's details by product id
		[HttpGet("details/{id}")]
		public async Task<ActionResult<ProductListDto>> GetSingleProductDto(int id)
		{
			var productDto = await _productServices.GetSingleProductDto(id);

			if (productDto == null)
			{
				return NotFound("Product not found");
			}

			return Ok(productDto);
		}

		//post products
		[HttpPost]
		public async Task<ActionResult<List<Product>>> AddProduct([FromForm] ProductDto productDto, IFormFile file)
		{
			if (file == null || file.Length == 0)
			{
				return BadRequest("No file uploaded.");
			}

			// upload file to the azure storage and get link
			var fileUrl = await _fileServices.Upload(file, AzureContainerName);

			// Map the DTO to the Product entity
			var product = _mapper.Map<Product>(productDto);

			//set the imageUrl from azure blob storage
			product.ProductImageUrl = fileUrl;

			// Add the product to the database
			var result = await _productServices.AddProduct(product);

			return Ok(result);
		}

		//update product
		[HttpPut("{id}")]
		public async Task<ActionResult<List<Product>>> UpdateProduct(int id, [FromForm] ProductDto productDto, IFormFile file)
		{
			// upload new file to the azure storage and get link
			var newFileUrl = await _fileServices.Upload(file, AzureContainerName);

			// Map the DTO to the Product entity
			var request = _mapper.Map<Product>(productDto);

			var result = await _productServices.UpdateProduct(id, request, newFileUrl);
			if (result is null)
				return NotFound("product is not found");

			return Ok(result);
		}

		//update product
		[HttpPut("update-image/{id}")]
		public async Task<ActionResult<List<Product>>> UpdateProductImage(int id, IFormFile file)
		{
			// upload new file to the azure storage and get link
			var newFileUrl = await _fileServices.Upload(file, AzureContainerName);
			var result = await _productServices.UpdateProductImage(id, newFileUrl);
			if (result is null)
				return NotFound("product is not found");

			return Ok("file uploaded");
		}

		//update product without product image
		[HttpPut("update/{id}")]
		public async Task<ActionResult<List<Product>>> UpdateProductDetails(int id, [FromForm] ProductDto productDto)
		{

			// Map the DTO to the Product entity
			var request = _mapper.Map<Product>(productDto);

			var result = await _productServices.UpdateProductDetails(id, request);
			if (result is null)
				return NotFound("product is not found");
			return Ok(result);
		}

		//delete products
		[HttpDelete("{id}")]
		public async Task<ActionResult<List<Product>>> DeleteProduct(int id)
		{
			var result = await _productServices.DeleteProduct(id);
			if (result is null)
				return NotFound("product is not found");
			return Ok(result);
		}

		//search products
        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> SearchProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            try
            {
                var products = await _productServices.SearchAsync(searchTerm);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}

