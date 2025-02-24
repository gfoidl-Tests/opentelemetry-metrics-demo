namespace BookStore.Domain.Models;

public class Book : Entity
{
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public double Value { get; set; }
    public DateTime PublishDate { get; set; }
    public int CategoryId { get; set; }

    public Category? Category { get; set; }
    public Inventory? Inventory { get; set; }
    public List<Order>? Orders { get; set; }

    public bool HasPositivePrice() => this.Value > 0;

    public bool HasCorrectPublishDate() => this.PublishDate < DateTime.Today;
}
