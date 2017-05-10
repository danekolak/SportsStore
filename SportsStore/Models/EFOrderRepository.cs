using Microsoft.EntityFrameworkCore;
using SportsStore.Data;
using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public EFOrderRepository(AppDbContext ctx)
        {
            _context = ctx;
        }

        public IEnumerable<Order> Orders
        {
            get
            {
                return _context.Orders.Include(o => o.Lines).ThenInclude(l => l.Product);
            }
        }

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();

        }
    }
}
