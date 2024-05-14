using WebShopNPT.Repositories;
using WebShopNPT.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShopNPT.EFRepository
{
    public class EFOrderRepo : IOrder
    {
        private readonly WebSiteDacsContext _context;
        public EFOrderRepo(WebSiteDacsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders.ToListAsync();
        }
    }
}
