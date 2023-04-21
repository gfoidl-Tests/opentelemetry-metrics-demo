namespace BookStore.Domain.Models;

public class Order : Entity
{
    public string? CustomerName { get; set; }
    public string? Address { get; set; }
    public string? Telephone { get; set; }
    public string? City { get; set; }
    public double TotalAmount { get; set; }
    public string? Status { get; set; }
    public List<Book>? Books { get; set; }

    public bool IsAlreadyCancelled() => this.Status?.Contains("CANCELLED", StringComparison.InvariantCultureIgnoreCase) is true;

    public void SetCancelledStatus() => this.Status = "CANCELLED";

    public void SetNewOrderStatus() => this.Status = "NEW_ORDER";

    public void SetTotalAmount(double amount) => this.TotalAmount = amount;
}
