using Mango.Sevices.OrderAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Sevices.OrderAPI.Repository
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(OrderHeader orderHeader);
        Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid);
        Task<IEnumerable<OrderHeader>> GetOrderByUserId(string userId);
        Task<IEnumerable<OrderHeader>> GetOrders();
    }
}
