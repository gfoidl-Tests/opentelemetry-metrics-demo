using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Infrastructure.Context;
using BookStore.Infrastructure.Metrics;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Repositories;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    private readonly OtelMetrics _meters;

    public OrderRepository(BookStoreDbContext context, OtelMetrics meters)
        : base(context)
        => _meters = meters;

    public override async Task<Order?> GetById(int id)
    {
        return await Db.Orders
            .Include(b => b.Books)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<List<Order>> GetAll()
    {
        return await Db.Orders
            .Include(b => b.Books)
            .ToListAsync();
    }

    public override async Task Add(Order entity)
    {
        DbSet.Add(entity);
        await this.SaveChanges();

        _meters.RecordOrderTotalPrice(entity.TotalAmount);
        _meters.RecordNumberOfBooks(entity.Books?.Count ?? 0);
        _meters.IncreaseTotalOrders(entity.City);
    }

    public override async Task Update(Order entity)
    {
        await base.Update(entity);

        _meters.IncreaseOrdersCanceled();
    }

    public async Task<List<Order>> GetOrdersByBookId(int bookId)
    {
        return await Db.Orders.AsNoTracking()
            .Include(b => b.Books)
            .Where(x => x.Books!.Any(y => y.Id == bookId))
            .ToListAsync();
    }
}
