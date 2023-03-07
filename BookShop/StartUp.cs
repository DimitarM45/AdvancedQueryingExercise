namespace BookShop;

using Data;
using Models;
using Initializer;
using Models.Enums;

using Z.EntityFramework.Plus;
using Microsoft.EntityFrameworkCore;

using System.Text;

public class StartUp
{
    static void Main(string[] args)
    {
        using BookShopContext context = new BookShopContext();

        DbInitializer.ResetDatabase(context);
    }

    //Problem 2

    public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    {
        try
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, false);

            var books = context.Books
                .AsNoTracking()
                .Where(b => b.AgeRestriction == ageRestriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var book in books)
                stringBuilder.AppendLine(book);

            return stringBuilder.ToString().TrimEnd();
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    //Problem 3 

    public static string GetGoldenBooks(BookShopContext context)
    {
        var books = context.Books
            .AsNoTracking()
            .Where(b => b.EditionType == EditionType.Gold
                     && b.Copies < 5000)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var book in books)
            stringBuilder.AppendLine(book);

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 4 

    public static string GetBooksByPrice(BookShopContext context)
    {
        var books = context.Books
            .AsNoTracking()
            .OrderByDescending(b => b.Price)
            .Where(b => b.Price > 40)
            .Select(b => new
            {
                b.Title,
                b.Price
            })
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var book in books)
            stringBuilder.AppendLine($"{book.Title} - ${book.Price:f2}");


        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 5

    public static string GetBooksNotReleasedIn(BookShopContext context, int year)
    {
        var books = context.Books
            .AsNoTracking()
            .Where(b => b.ReleaseDate!.Value.Year != year)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var book in books)
            stringBuilder.AppendLine(book);

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 6

    public static string GetBooksByCategory(BookShopContext context, string input)
    {
        string[] categories = input
            .ToLower()
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        var books = context.Books
            .AsNoTracking()
            .Where(b => b.BookCategories.
                Any(bc => categories.Contains(bc.Category.Name.ToLower())))
            .OrderBy(b => b.Title)
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var book in books)
            stringBuilder.AppendLine(book.Title);

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 7

    public static string GetBooksReleasedBefore(BookShopContext context, string date)
    {
        DateTime parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

        var books = context.Books
            .AsNoTracking()
            .Where(b => b.ReleaseDate < parsedDate)
            .OrderByDescending(b => b.ReleaseDate)
            .Select(b => new
            {
                b.Title,
                EditionType = b.EditionType.ToString(),
                b.Price
            })
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var book in books)
            stringBuilder.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price}");

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 8

    public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
    {
        var authors = context.Authors
            .AsNoTracking()
            .Where(a => a.FirstName.EndsWith(input))
            .OrderBy(a => a.FirstName)
            .ThenBy(a => a.LastName)
            .Select(a => $"{a.FirstName} {a.LastName}")
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var author in authors)
            stringBuilder.AppendLine(author);

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 9

    public static string GetBookTitlesContaining(BookShopContext context, string input)
    {
        var books = context.Books
            .AsNoTracking()
            .Where(b => b.Title.ToLower().Contains(input.ToLower()))
            .OrderBy(b => b.Title)
            .Select(b => b.Title)
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var book in books)
            stringBuilder.AppendLine(book);

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 10

    public static string GetBooksByAuthor(BookShopContext context, string input)
    {
        var books = context.Books
            .AsNoTracking()
            .Where(b => b.Author.LastName
                .ToLower()
                .StartsWith(input.ToLower()))
            .OrderBy(b => b.BookId)
            .Select(b => new
            {
                b.Title,
                AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}"
            })
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var book in books)
            stringBuilder.AppendLine($"{book.Title} ({book.AuthorFullName})");

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 11

    public static int CountBooks(BookShopContext context, int lengthCheck)
    {
        int booksCount = context.Books
            .Count(b => b.Title.Length > lengthCheck);

        return booksCount;
    }

    //Problem 12

    public static string CountCopiesByAuthor(BookShopContext context)
    {
        var authorsByBookCount = context.Authors
            .AsNoTracking()
            .Select(a => new
            {
                FullName = $"{a.FirstName} {a.LastName}",
                BookCopyCount = a.Books
                    .Sum(b => b.Copies)
            })
            .ToArray()
            .OrderByDescending(a => a.BookCopyCount);

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var author in authorsByBookCount)
            stringBuilder.AppendLine($"{author.FullName} - {author.BookCopyCount}");

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 13

    public static string GetTotalProfitByCategory(BookShopContext context)
    {
        var categoriesByProfit = context.Categories
            .AsNoTracking()
            .Select(c => new
            {
                c.Name,
                TotalProfit = c.CategoryBooks
                    .Sum(cb => cb.Book.Copies * cb.Book.Price)
            })
            .OrderByDescending(c => c.TotalProfit)
            .ThenBy(c => c.Name);

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var category in categoriesByProfit)
            stringBuilder.AppendLine($"{category.Name} ${category.TotalProfit:f2}");

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 14

    public static string GetMostRecentBooks(BookShopContext context)
    {
        var categories = context.Categories
            .AsNoTracking()
            .Select(c => new
            {
                c.Name,
                MostRecentBooks = c.CategoryBooks
                    .OrderByDescending(cb => cb.Book.ReleaseDate.Value)
                    .Select(cb => new
                    {
                        BookTitle = cb.Book.Title,
                        BookReleaseYear = cb.Book.ReleaseDate.Value.Year
                    })
                    .Take(3)
                    .ToArray()
            })
            .OrderBy(c => c.Name)
            .ToArray();

        StringBuilder stringBuilder = new StringBuilder();

        foreach (var category in categories)
        {
            stringBuilder.AppendLine($"--{category.Name}");

            foreach (var book in category.MostRecentBooks)
                stringBuilder.AppendLine($"{book.BookTitle} ({book.BookReleaseYear})");
        }

        return stringBuilder.ToString().TrimEnd();
    }

    //Problem 15

    public static void IncreasePrices(BookShopContext context)
    {
        int year = 2010;
        int increase = 5;

        context.Books
            .Where(b => b.ReleaseDate.HasValue
                     && b.ReleaseDate.Value.Year < year)
            .Update(b => new Book { Price = b.Price + increase });
    }

    //Problem 16

    public static int RemoveBooks(BookShopContext context)
    {
        int copies = 4200;

        int deletedBooks = context.Books
            .Where(b => b.Copies < copies)
            .Delete();

        return deletedBooks;
    }
}