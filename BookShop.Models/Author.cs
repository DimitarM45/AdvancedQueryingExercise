namespace BookShop.Models;

using Data.Common;

using System.ComponentModel.DataAnnotations;

public class Author
{
    public Author()
    {
        Books = new HashSet<Book>();    
    }

    public int AuthorId { get; set; }

    [MaxLength(ValidationConstants.AuthorFirstNameMaxLength)]

    public string? FirstName { get; set; }

    [Required]
    [MaxLength(ValidationConstants.AuthorLastNameMaxLength)]

    public string LastName { get; set; } = null!;

    public ICollection<Book> Books { get; set; }
}