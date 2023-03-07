namespace BookShop.Models;

using Enums;
using Data.Common;

using System.ComponentModel.DataAnnotations;

public class Book
{
    public Book()
    {
        BookCategories = new HashSet<BookCategory>();    
    }

    public int BookId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.BookTitleMaxLength)]

    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(ValidationConstants.BookDescriptionMaxLength)]

    public string Description { get; set; } = null!;

    public DateTime? ReleaseDate { get; set; }

    public int Copies { get; set; }

    public decimal Price { get; set; }

    public EditionType EditionType { get; set; }

    public AgeRestriction AgeRestriction { get; set; }

    public int AuthorId { get; set; }

    public Author Author { get; set; } = null!;

    public ICollection<BookCategory> BookCategories { get; set; }
}
