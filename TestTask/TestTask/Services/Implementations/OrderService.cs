using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<Order> GetOrder()
        {
            var minQuantity = 1;
            var order = await _context.Orders
                                    .OrderByDescending(o => o.CreatedAt)
                                    .FirstOrDefaultAsync(o => o.Quantity > minQuantity);
            return order;
        }

        public async Task<List<Order>> GetOrders()
        {
            var orders = await _context.Orders
                                      .Where(o => o.User.Status == Enums.UserStatus.Active)
                                      .OrderByDescending(o => o.CreatedAt)
                                      .ToListAsync();
            return orders;
        }
    }
}
