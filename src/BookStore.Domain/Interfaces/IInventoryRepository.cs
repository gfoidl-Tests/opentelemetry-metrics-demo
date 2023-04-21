using BookStore.Domain.Models;

namespace BookStore.Domain.Interfaces;

public interface IInventoryRepository : IRepository<Inventory>
{
    Task<IEnumerable<Inventory>> SearchInventoryForBook(string bookName);
}
