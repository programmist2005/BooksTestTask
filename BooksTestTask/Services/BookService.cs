using BooksTestTask.DataAccess.Contexts;
using BooksTestTask.DomainModels;
using BooksTestTask.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BooksTestTask.Services;

public class BookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .FromSqlRaw("SELECT * FROM usp_books_get_by_id({0})", id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    public async Task<DetailBook?> GetDetailByIdAsync(int id)
    {
        var book = await GetByIdAsync(id);
        
        return book == null ? null : new DetailBook
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            YearPublished = book.YearPublished,
            TocXml = book.TocXml,
            CreatedAt = book.CreatedAt,
            UpdatedAt = book.UpdatedAt
        };
    }
    public async Task<DeleteBook?> GetDeleteByIdAsync(int id)
    {
        var book = await GetByIdAsync(id);

        return book == null ? null : new DeleteBook
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            YearPublished = book.YearPublished
        };
    }
    public async Task<EditBook?> GetEditByIdAsync(int id)
    {
        var book = await GetByIdAsync(id);

        return book == null ? null : new EditBook
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            YearPublished = book.YearPublished,
            TocXml = book.TocXml
        };
    }

    public async Task<List<ShortInfoBook>> SearchAsync(string? title = null, string? author = null, string? tocText = null)
    {
        var books = await _context.Books
            .FromSqlRaw("SELECT * FROM usp_books_search({0}, {1}, {2})", title, author, tocText)
            .AsNoTracking()
            .ToListAsync();

        return books.Select(b => new ShortInfoBook
        {
            Id = b.Id,
            Title = b.Title,
            Author = b.Author,
            YearPublished = b.YearPublished
        }).ToList();
    }

    public async Task<int> CreateAsync(CreateBook book)
    {
        var result = await _context.Database
            .SqlQueryRaw<int>(
                @"SELECT usp_books_insert({0}, {1}, {2}, {3}) AS ""Value""",
                book.Title,
                book.Author,
                book.YearPublished,
                book.TocXml ?? (object)DBNull.Value)
            .ToListAsync();

        return result.First();
    }

    public async Task UpdateAsync(EditBook book)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "SELECT usp_books_update({0}, {1}, {2}, {3}, {4})",
            book.Id, book.Title, book.Author, book.YearPublished, book.TocXml);
    }

    public async Task DeleteAsync(int id)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "SELECT usp_books_delete({0})", id);
    }
}