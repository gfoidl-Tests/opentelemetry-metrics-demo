namespace BookStore.Domain.Models;

public class Inventory : Entity
{
    public int Amount { get; set; }
    public virtual Book? Book { get; set; }

    public bool HasInventoryAvailable() => this.Amount > 0;

    public void DecreaseInventory() => --this.Amount;

    public void IncreaseInventory() => ++this.Amount;
}
