using WebShopNPT.Models;

namespace WebShopNPT.Repositories
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetAllAsync();
    }
}
