using BookStore.Domain.Models;

namespace BookStore.Domain.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<Order>> GetAll();
    Task<Order?> GetById(int id);
    Task<Order?> Add(Order order);
    Task<Order?> Remove(Order order);
}
