using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<User> GetUser()
        {
            var yearOfCreation = 2003;
            var userQuery = await _context.Users
                                .Select(user
                                            => new { user, 
                                                     totalOrderSum = user.Orders
                                                                             .Where(o => o.Status == Enums.OrderStatus.Delivered && o.CreatedAt.Year == yearOfCreation)
                                                                             .Sum(o => o.Quantity * o.Price) })
                                .OrderByDescending(data => data.totalOrderSum)
                                .FirstOrDefaultAsync();
            return  userQuery.user;
        }

        public Task<List<User>> GetUsers()
        {
            var yearOfCreation = 2010;
            var users = _context.Users
                                    .Where(u => u.Orders.FirstOrDefault(o => o.Status == Enums.OrderStatus.Paid 
                                                                            && o.CreatedAt.Year == yearOfCreation) != null)
                                    .ToListAsync();
            return users;
        }
    }
}
