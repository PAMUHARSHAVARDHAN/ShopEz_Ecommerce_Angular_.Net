using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productservice.DTO;
using Productservice.Models;
using Productservice.Services;


namespace Productservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // User Products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllProductsAsync();

            return Ok(products);
        }

        // Product By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);

            if (product == null)
                return NotFound(new
                {
                    message = "Product not found"
                });

            return Ok(product);
        }

        // Admin Products
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var products = await _service.GetAllAdminProductsAsync();

            return Ok(products);
        }

        // Create Product
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product =
                await _service.CreateProductAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = product.ProductId },
                product);
        }

        // Update Product
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] ProductDTO dto)
        {
            var result =
                await _service.UpdateProductAsync(id, dto);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            return Ok(new
            {
                message = "Product updated successfully"
            });
        }
        [HttpPut("decrease-stock/{id}")]
        public async Task<IActionResult>
    DecreaseStock(
        int id,
        [FromBody] int quantity)
        {
            var product =
                await _service.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            if (product.Stock < quantity)
            {
                return BadRequest(new
                {
                    message =
                    $"Only {product.Stock} items available"
                });
            }

            // Reduce Stock
            product.Stock -= quantity;

            // Create DTO
            var dto = new ProductDTO
            {
                Name = product.Name,

                Price = product.Price,

                Description = product.Description,

                Stock = product.Stock,

                CategoryId = product.CategoryId,

                Gender = product.Gender,

                ProductImages = product.ProductImages
                    .Select(pi => new ProductImageDTO
                    {
                        ImageUrl = pi.ImageUrl
                    }).ToList(),

                ProductSizes = product.ProductSizes
                    .Select(ps => new ProductSizeDTO
                    {
                        SizeValue = ps.SizeValue,
                        Stock = ps.Stock
                    }).ToList()
            };

            // Update Product
            await _service.UpdateProductAsync(id, dto);

            return Ok(new
            {
                message = "Stock updated successfully",

                remainingStock = product.Stock
            });
        }


        // Soft Delete Product
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result =
                await _service.DeleteProductAsync(id);

            if (!result)
            {
                return NotFound(new
                {
                    message = "Product not found"
                });
            }

            return Ok(new
            {
                message = "Product deleted successfully"
            });
        }

        // Upload Product Image
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file,    
            [FromForm] string category,
            [FromForm] string productFolder)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new
                {
                    message = "No file uploaded"
                });
            }

            // Unique File Name
            var fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(file.FileName);

            // Folder Path
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/Productimages", category,
    productFolder);

            // Create Folder
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath =
                Path.Combine(folderPath, fileName);

            // Save File
            using (var stream =
                new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imagePath =
                 $"{category}/{productFolder}/{fileName}";
            return Ok(new
            {
                imageUrl = imagePath
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple(
    List<IFormFile> files,
    [FromForm] string category,
    [FromForm] string productFolder)
        {

            if (files == null || files.Count == 0)
            {
                return BadRequest(new
                {
                    message = "No files uploaded"
                });
            }

            // Folder Path
            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/Productimages",
                category,
                productFolder
            );

            // Create Folder
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var uploadedImages = new List<object>();

            foreach (var file in files)
            {

                // Unique file name
                var fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(file.FileName);

                var filePath =
                    Path.Combine(folderPath, fileName);

                // Save file
                using (var stream =
                    new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Image URL
                var imagePath =
                    $"{category}/{productFolder}/{fileName}";

                uploadedImages.Add(new
                {
                    imageUrl = imagePath
                });

            }

            return Ok(new
            {
                productImages = uploadedImages
            });

        }
    }
}