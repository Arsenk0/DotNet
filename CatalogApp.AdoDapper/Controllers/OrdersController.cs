using CatalogApp.AdoDapper.Data;
using CatalogApp.AdoDapper.Data.Entities;
using CatalogApp.AdoDapper.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApp.AdoDapper.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            // Валідація спрацює автоматично завдяки FluentValidation
            try
            {
                _unitOfWork.BeginTransaction();

                var order = new Order
                {
                    CustomerName = dto.CustomerName,
                    OrderDate = DateTime.UtcNow,
                    Status = "New",
                    Items = new List<OrderItem>()
                };

                decimal total = 0;

                foreach (var itemDto in dto.Items)
                {
                    var product = await _unitOfWork.Products.GetByIdAsync(itemDto.ProductId);
                    if (product == null) throw new Exception($"Товар {itemDto.ProductId} не знайдено");

                    // Приведення dynamic до типів
                    var dict = (IDictionary<string, object>)product;
                    string pName = dict["Name"].ToString()!;
                    decimal pPrice = Convert.ToDecimal(dict["Price"]);

                    order.Items.Add(new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        ProductName = pName,
                        UnitPrice = pPrice,
                        Quantity = itemDto.Quantity
                    });
                    total += pPrice * itemDto.Quantity;
                }

                order.TotalAmount = total;
                var orderId = _unitOfWork.Orders.Create(order);
                _unitOfWork.Commit();

                return CreatedAtAction(nameof(GetOrder), new { id = orderId }, new { OrderId = orderId });
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null) return NotFound();

                await _unitOfWork.Orders.UpdateStatusAsync(id, status);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var order = await _unitOfWork.Orders.GetByIdAsync(id);
                if (order == null) return NotFound();

                await _unitOfWork.Orders.DeleteAsync(id);
                _unitOfWork.Commit();
                return NoContent();
            }
            catch
            {
                _unitOfWork.Rollback();
                return BadRequest();
            }
        }
    }

    public class CreateOrderDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }

    public class CreateOrderItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}