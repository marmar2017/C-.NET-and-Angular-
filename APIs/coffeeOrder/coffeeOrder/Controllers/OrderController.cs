using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coffeeOrder.Data;
using coffeeOrder.Models;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace coffeeOrder.Controllers
{
    [ApiController]
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly CoffeeOrderDbContext _context;

        public OrdersController(CoffeeOrderDbContext context)
        {
            _context = context;
        }

        // POST/order
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([Required][FromBody] CoffeeOrderPayload payload)
        {
            if (payload == null)
                return BadRequest("Order payload is required.");

            var order = new Order
            {
                Id = Guid.NewGuid(),
                PayloadJson = JsonSerializer.Serialize(payload),
                Status = OrderStatus.Pending,
                RetryCount = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }



        // GET /orders
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]                     
        [ProducesResponseType(StatusCodes.Status404NotFound)]      
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // GET /orders/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Order>> GetOrderById(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            return order;
        }



        // DELETE /orders
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAllOrders()
        {
            var orders = await _context.Orders.ToListAsync();

            if (orders.Count == 0)
                return NoContent();

            _context.Orders.RemoveRange(orders);
            await _context.SaveChangesAsync();

            return NoContent();  
        }

    }
}
