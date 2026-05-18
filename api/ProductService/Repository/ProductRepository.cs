using Dapper;
using Microsoft.Data.SqlClient;
using Productservice.Models;

namespace Productservice.Repository
{
    public class ProductRepository:IProductRepository
    {
        private readonly string _connStr;

        public ProductRepository(IConfiguration config)
        {
            _connStr =
                config.GetConnectionString(
                    "DefaultConnection")!;
        }

        // Create SQL Connection
        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connStr);
        }

        // User Products
        public async Task<List<Product>>
            GetAllAsync()
        {
            var query = @"
            SELECT
                p.ProductId,
                p.Name,
                p.Price,
                p.Description,
                p.Stock,
                p.IsActive,
                p.CategoryId,
                p.Gender,
                c.Name AS CategoryName
            FROM Products p
            INNER JOIN Categories c
                ON p.CategoryId = c.CategoryId
            WHERE p.IsActive = 1";

            using var connection = GetConnection();

            var products =
                (await connection.QueryAsync<Product>(
                    query)).ToList();

            var imageQuery = @"
SELECT *
FROM ProductImages";

            var allImages =
                (await connection.QueryAsync<ProductImage>(
                    imageQuery)).ToList();

            var sizeQuery = @"
SELECT *
FROM ProductSizes";

            var allSizes =
                (await connection.QueryAsync<ProductSize>(
                    sizeQuery)).ToList();

            foreach (var product in products)
            {
                product.ProductImages =
                    allImages
                    .Where(i => i.ProductId == product.ProductId)
                    .ToList();

                product.ProductSizes =
                    allSizes
                    .Where(s => s.ProductId == product.ProductId)
                    .ToList();
            }
            return products;
            }

        // Admin Products
        public async Task<List<Product>>
            GetAllAdminAsync()
        {
            var query = @"
            SELECT
                p.ProductId,
                p.Name,
                p.Price,
                p.Description,
                p.Stock,
                p.IsActive,
                p.CategoryId,
                p.Gender,
                c.Name AS CategoryName
            FROM Products p
            INNER JOIN Categories c
                ON p.CategoryId = c.CategoryId";

            using var connection = GetConnection();

            var products =
                (await connection.QueryAsync<Product>(
                    query)).ToList();
            var imageQuery = @"
SELECT *
FROM ProductImages";

            var allImages =
                (await connection.QueryAsync<ProductImage>(
                    imageQuery)).ToList();

            var sizeQuery = @"
SELECT *
FROM ProductSizes";

            var allSizes =
                (await connection.QueryAsync<ProductSize>(
                    sizeQuery)).ToList();

            foreach (var product in products)
            {
                product.ProductImages =
                    allImages
                    .Where(i => i.ProductId == product.ProductId)
                    .ToList();

                product.ProductSizes =
                    allSizes
                    .Where(s => s.ProductId == product.ProductId)
                    .ToList();
            }

            return products;

        }

        // Product By Id
        public async Task<Product?>
            GetByIdAsync(int id)
        {
            var query = @"
            SELECT
                p.ProductId,
                p.Name,
                p.Price,
                p.Description,
                p.Stock,
                p.IsActive,
                p.CategoryId,
                p.Gender,
                c.Name AS CategoryName
            FROM Products p
            INNER JOIN Categories c
                ON p.CategoryId = c.CategoryId
            WHERE p.ProductId = @Id";

            using var connection = GetConnection();

            var product =
                 await connection
                     .QueryFirstOrDefaultAsync<Product>(
                         query,
                         new { Id = id });

            if (product == null)
                return null;

            // Load Images
            var imageQuery = @"
            SELECT ImageId, ProductId, ImageUrl
            FROM ProductImages
            WHERE ProductId = @Id";

            var images =
                await connection.QueryAsync<ProductImage>(
                    imageQuery,
                    new { Id = id });

            product.ProductImages = images.ToList();

            // Load Sizes
            var sizeQuery = @"
            SELECT ProductSizeId, ProductId, SizeValue, Stock
            FROM ProductSizes
            WHERE ProductId = @Id";

            var sizes =
                await connection.QueryAsync<ProductSize>(
                    sizeQuery,
                    new { Id = id });

            product.ProductSizes = sizes.ToList();

            return product;
        }

        // Add Product
        public async Task<Product>
            AddAsync(Product product)
        {
            var query = @"
            INSERT INTO Products
            (
                Name,
                Price,
                Description,
                Stock,
                IsActive,
                CategoryId,
                Gender
            )
            VALUES
            (
                @Name,
                @Price,
                @Description,
                @Stock,
                @IsActive,
                @CategoryId,
                @Gender
            );

            SELECT CAST(SCOPE_IDENTITY() as int)";

            using var connection = GetConnection();

            var productId =
                await connection.ExecuteScalarAsync<int>(
                    query,
                    product);
            // Insert Multiple Images
            foreach (var image in product.ProductImages)
            {
                var imageQuery = @"
                INSERT INTO ProductImages
                (
                    ProductId,
                    ImageUrl
                )
                VALUES
                (
                    @ProductId,
                    @ImageUrl
                )";

                await connection.ExecuteAsync(
                    imageQuery,
                    new
                    {
                        ProductId = productId,
                        ImageUrl = image.ImageUrl
                    });
            }
            // Insert Sizes
            foreach (var size in product.ProductSizes)
            {
                var sizeQuery = @"
                INSERT INTO ProductSizes
                (
                    ProductId,
                    SizeValue,
                    Stock
                )
                VALUES
                (
                    @ProductId,
                    @SizeValue,
                    @Stock
                )";

                await connection.ExecuteAsync(
                    sizeQuery,
                    new
                    {
                        ProductId = productId,
                        SizeValue = size.SizeValue,
                        Stock = size.Stock
                    });
            }


            return await GetByIdAsync(productId)
          ?? throw new Exception("Product not found");


        }

        // Update Product
        public async Task<Product?>
            UpdateAsync(Product product)
        {
            var query = @"
            UPDATE Products
            SET
                Name = @Name,
                Price = @Price,
                Description = @Description,
                Stock = @Stock,
                IsActive = @IsActive,
                CategoryId = @CategoryId,
                Gender = @Gender
            WHERE ProductId = @ProductId";

            using var connection = GetConnection();

            var rowsAffected =
                await connection.ExecuteAsync(
                    query,
                    product);

            if (rowsAffected == 0)
                return null;
            // Delete Old Images
            var deleteImagesQuery = @"
            DELETE FROM ProductImages
            WHERE ProductId = @ProductId";

            await connection.ExecuteAsync(
                deleteImagesQuery,
                new { product.ProductId });
            // Delete Old Sizes
            var deleteSizesQuery = @"
            DELETE FROM ProductSizes
            WHERE ProductId = @ProductId";

            await connection.ExecuteAsync(
                deleteSizesQuery,
                new { product.ProductId });

            // Insert New Images
            foreach (var image in product.ProductImages)
            {
                var imageQuery = @"
                INSERT INTO ProductImages
                (
                    ProductId,
                    ImageUrl
                )
                VALUES
                (
                    @ProductId,
                    @ImageUrl
                )";

                await connection.ExecuteAsync(
                    imageQuery,
                    new
                    {
                        ProductId = product.ProductId,
                        ImageUrl = image.ImageUrl
                    });
            }
            foreach (var size in product.ProductSizes)
            {
                var sizeQuery = @"
                INSERT INTO ProductSizes
                (
                    ProductId,
                    SizeValue,
                    Stock
                )
                VALUES
                (
                    @ProductId,
                    @SizeValue,
                    @Stock
                )";

                await connection.ExecuteAsync(
                    sizeQuery,
                    new
                    {
                        ProductId = product.ProductId,
                        SizeValue = size.SizeValue,
                        Stock = size.Stock
                    });
            }

            return await GetByIdAsync(product.ProductId);

        }

        // Delete Product
        public async Task<bool>
            DeleteAsync(int id)
        {
             using var connection = GetConnection();

            // Delete Images First
            var deleteImagesQuery = @"
            DELETE FROM ProductImages
            WHERE ProductId = @Id";

            await connection.ExecuteAsync(
                deleteImagesQuery,
                new { Id = id });
            // Delete Sizes
            var deleteSizesQuery = @"
            DELETE FROM ProductSizes
            WHERE ProductId = @Id";

            await connection.ExecuteAsync(
                deleteSizesQuery,
                new { Id = id });

            // Delete Product
            var query =
                "DELETE FROM Products WHERE ProductId = @Id";

            var rowsAffected =
                await connection.ExecuteAsync(
                    query,
                    new { Id = id });

            return rowsAffected > 0;
        }

    }
}
