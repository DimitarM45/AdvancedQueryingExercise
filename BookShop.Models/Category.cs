namespace BookShop.Models;

using Data.Common;

using System.ComponentModel.DataAnnotations;

public class Category
{
    public Category()
    {
        CategoryBooks = new HashSet<BookCategory>();    
    }

    public int CategoryId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.CategoryNameMaxLength)]

    public string Name { get; set; } = null!;

    public ICollection<BookCategory> CategoryBooks { get; set; }
}
