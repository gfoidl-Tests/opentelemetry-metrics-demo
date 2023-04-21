using System.ComponentModel.DataAnnotations;

namespace BookStore.WebApi.Dtos.Order;

public class OrderAddDto
{
    [Required(ErrorMessage = "The field {0} is required")]
    [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
    public string CustomerName { get; set; } = null!;

    [Required(ErrorMessage = "The field {0} is required")]
    [StringLength(150, ErrorMessage = "The field {0} must be between {2} and {1} characters", MinimumLength = 2)]
    public string Address { get; set; } = null!;

    [Required(ErrorMessage = "The field {0} is required")]
    public string Telephone { get; set; } = null!;

    [Required(ErrorMessage = "The field {0} is required")]
    public string City { get; set; } = null!;

    [Required(ErrorMessage = "The field {0} is required")]
    public IEnumerable<int> Books { get; set; } = null!;
}
