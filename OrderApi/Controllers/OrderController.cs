using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RabbitMQ.Client;
using OrderApi.Model;
using Microsoft.EntityFrameworkCore;

namespace OrderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        OrderContext _context;

        public OrderController(OrderContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetTodoItems()
        {
            return await _context.OrderItems
             .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderItem>> GetOrderItem(long id)
        {
            var OrderItem = await _context.OrderItems.FindAsync(id);

            if (OrderItem == null)
            {
                return NotFound();
            }

            return OrderItem;
        }

        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
          /*  var factory = new ConnectionFactory() { HostName = "localhost", Port = 5673 };
         
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
                    string message = "New Order Created!";
                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish(exchange: "",
                                 routingKey: "hello",
                                 basicProperties: null,
                                 body: body);
                }
            }
            */
            return CreatedAtAction("GetOrderItem", new { id = orderItem.Id }, orderItem);
            //return CreatedAtAction(nameof(orderItem), new { id = orderItem.Id }, orderItem);
        }

        // PUT: api/TodoItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool OrderItemExists(long id)
        {
            return false;
        }

    }
}