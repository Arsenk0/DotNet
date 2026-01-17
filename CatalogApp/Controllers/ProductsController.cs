using AutoMapper;
using CatalogApp.Dtos;
using CatalogApp.Interfaces;
using CatalogApp.Models;
using CatalogApp.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<Category> _categoriesRepo;
        private readonly IMapper _mapper;

        public ProductsController(
            IGenericRepository<Product> productsRepo,
            IGenericRepository<Category> categoriesRepo,
            IMapper mapper)
        {
            _productsRepo = productsRepo;
            _categoriesRepo = categoriesRepo;
            _mapper = mapper;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductWithCategorySpecification(productParams);
            var products = await _productsRepo.ListAsync(spec);
            
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductDto>>(products);
            return Ok(data);
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var spec = new ProductWithCategorySpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new { Message = "Product not found" });

            return Ok(_mapper.Map<Product, ProductDto>(product));
        }

        // GET: api/products/categories
        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategories()
        {
            var categories = await _categoriesRepo.ListAllAsync();
            return Ok(categories);
        }

        // POST: api/products (Створення)
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductCreateDto productToCreate)
        {
            var product = _mapper.Map<ProductCreateDto, Product>(productToCreate);
            
            _productsRepo.Add(product);
            
            if (await _productsRepo.SaveAllAsync())
            {
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, _mapper.Map<ProductDto>(product));
            }

            return BadRequest("Problem creating product");
        }

        // PUT: api/products/5 (Оновлення) <--- НОВИЙ МЕТОД
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, ProductCreateDto productToUpdate)
        {
            // 1. Спочатку шукаємо товар у базі
            var product = await _productsRepo.GetByIdAsync(id);

            if (product == null) return NotFound(new { Message = "Product not found" });

            // 2. Мапимо нові дані поверх старих
            // AutoMapper візьме дані з productToUpdate і запише їх у product
            _mapper.Map(productToUpdate, product);
            
            // 3. Позначаємо як змінений
            _productsRepo.Update(product);

            if (await _productsRepo.SaveAllAsync()) return NoContent(); // 204 No Content - стандарт для Update

            return BadRequest("Failed to update product");
        }

        // DELETE: api/products/5 (Видалення) <--- НОВИЙ МЕТОД
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _productsRepo.GetByIdAsync(id);

            if (product == null) return NotFound(new { Message = "Product not found" });

            _productsRepo.Delete(product);

            if (await _productsRepo.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to delete product");
        }
    }
}