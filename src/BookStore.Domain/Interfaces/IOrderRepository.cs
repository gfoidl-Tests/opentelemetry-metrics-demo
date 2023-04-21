using BookStore.Domain.Models;

namespace BookStore.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<List<Order>> GetOrdersByBookId(int bookId);
}
