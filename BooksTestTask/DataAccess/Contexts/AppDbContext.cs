using BooksTestTask.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask.DataAccess.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; } = null!;
}