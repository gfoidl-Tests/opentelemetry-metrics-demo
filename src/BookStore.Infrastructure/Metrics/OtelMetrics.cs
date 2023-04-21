using System.Diagnostics.Metrics;

namespace BookStore.Infrastructure.Metrics;

public class OtelMetrics
{
    //Books meters
    private Counter<int> BooksAddedCounter { get; }
    private Counter<int> BooksDeletedCounter { get; }
    private Counter<int> BooksUpdatedCounter { get; }
    private UpDownCounter<int> TotalBooksUpDownCounter { get; }

    //Categories meters
    private Counter<int> CategoriesAddedCounter { get; }
    private Counter<int> CategoriesDeletedCounter { get; }
    private Counter<int> CategoriesUpdatedCounter { get; }
    private ObservableGauge<int> TotalCategoriesGauge { get; }
    private int _totalCategories = 0;

    //Order meters
    private Histogram<double> OrdersPriceHistogram { get; }
    private Histogram<int> NumberOfBooksPerOrderHistogram { get; }
    private ObservableCounter<int> OrdersCanceledCounter { get; }
    private int _ordersCanceled = 0;
    private Counter<int> TotalOrdersCounter { get; }

    public string MetricName { get; }

    public OtelMetrics(string meterName = "BookStore")
    {
        Meter meter = new(meterName);
        this.MetricName = meterName;

        this.BooksAddedCounter = meter.CreateCounter<int>("books-added", "Book");
        this.BooksDeletedCounter = meter.CreateCounter<int>("books-deleted", "Book");
        this.BooksUpdatedCounter = meter.CreateCounter<int>("books-updated", "Book");
        this.TotalBooksUpDownCounter = meter.CreateUpDownCounter<int>("total-books", "Book");

        this.CategoriesAddedCounter = meter.CreateCounter<int>("categories-added", "Category");
        this.CategoriesDeletedCounter = meter.CreateCounter<int>("categories-deleted", "Category");
        this.CategoriesUpdatedCounter = meter.CreateCounter<int>("categories-updated", "Category");
        this.TotalCategoriesGauge = meter.CreateObservableGauge<int>("total-categories", () => Volatile.Read(ref _totalCategories));

        this.OrdersPriceHistogram = meter.CreateHistogram<double>("orders-price", "Euros", "Price distribution of book orders");
        this.NumberOfBooksPerOrderHistogram = meter.CreateHistogram<int>("orders-number-of-books", "Books", "Number of books per order");
        this.OrdersCanceledCounter = meter.CreateObservableCounter<int>("orders-canceled", () => Volatile.Read(ref _ordersCanceled));
        this.TotalOrdersCounter = meter.CreateCounter<int>("total-orders", "Orders");
    }

    //Books meters
    public void AddBook() => this.BooksAddedCounter.Add(1);
    public void DeleteBook() => this.BooksDeletedCounter.Add(1);
    public void UpdateBook() => this.BooksUpdatedCounter.Add(1);
    public void IncreaseTotalBooks() => this.TotalBooksUpDownCounter.Add(1);
    public void DecreaseTotalBooks() => this.TotalBooksUpDownCounter.Add(-1);

    //Categories meters
    public void AddCategory() => this.CategoriesAddedCounter.Add(1);
    public void DeleteCategory() => this.CategoriesDeletedCounter.Add(1);
    public void UpdateCategory() => this.CategoriesUpdatedCounter.Add(1);
    public void IncreaseTotalCategories() => Interlocked.Increment(ref _totalCategories);
    public void DecreaseTotalCategories() => Interlocked.Decrement(ref _totalCategories);

    //Orders meters
    public void RecordOrderTotalPrice(double price) => this.OrdersPriceHistogram.Record(price);
    public void RecordNumberOfBooks(int amount) => this.NumberOfBooksPerOrderHistogram.Record(amount);
    public void IncreaseOrdersCanceled() => Interlocked.Increment(ref _ordersCanceled);
    public void IncreaseTotalOrders(string? city) => this.TotalOrdersCounter.Add(1, KeyValuePair.Create<string, object?>("City", city));
}
