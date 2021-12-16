using Mango.Sevices.OrderAPI.DbContexts;
using Mango.Sevices.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Sevices.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContext;

        public OrderRepository(DbContextOptions<ApplicationDbContext> dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> AddOrder(OrderHeader orderHeader)
        {
            await using var _context = new ApplicationDbContext(_dbContext);
            _context.OrderHeaders.Add(orderHeader);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<OrderHeader>> GetOrderByUserId(string userId)
        {
            await using var _context = new ApplicationDbContext(_dbContext);
            var result = await _context.OrderHeaders.Where(c=>c.UserId==userId).Include("OrderDetails").ToListAsync();
            return result;
        }

        public async Task<IEnumerable<OrderHeader>> GetOrders()
        {
            await using var _context  = new ApplicationDbContext(_dbContext);
            var result = await _context.OrderHeaders.Include("OrderDetails").ToListAsync();
            return result;

        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _db = new ApplicationDbContext(_dbContext);
            var orderHeaderFromDb = await _db.OrderHeaders.FirstOrDefaultAsync(u => u.OrderHeaderId == orderHeaderId);
            if (orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus = paid;
                await _db.SaveChangesAsync();
            }
        }
    }
}
