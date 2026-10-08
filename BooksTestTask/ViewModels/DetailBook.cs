namespace BooksTestTask.ViewModels;

public class DetailBook
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int YearPublished { get; set; }
    public string? TocXml { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
