namespace BookShop.Data;

using Models;
using Common;

using Microsoft.EntityFrameworkCore;

public class BookShopContext : DbContext
{
    public BookShopContext() 
    { 
    }

    public BookShopContext(DbContextOptions options)
        : base(options) 
    { 
    }

    public DbSet<Author> Authors { get; set; } = null!;

    public DbSet<Book> Books { get; set; } = null!;

    public DbSet<Category> Categories { get; set; } = null!;

    public DbSet<BookCategory> BooksCategories { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(Configuration.ConnectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BookCategory>()
            .HasKey(bk => new { bk.BookId, bk.CategoryId });
    }
}