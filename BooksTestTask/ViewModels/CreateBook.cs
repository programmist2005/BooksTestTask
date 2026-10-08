namespace BooksTestTask.ViewModels;

public class CreateBook
{
    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public int YearPublished { get; set; }
    public string? TocXml { get; set; }
}
