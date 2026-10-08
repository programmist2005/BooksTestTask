using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BooksTestTask.DomainModels;

[Table("books")]
public class Book
{
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title", TypeName = "character varying")]
    [MaxLength(500)]
    public string Title { get; set; } = null!;

    [Required]
    [Column("author", TypeName = "character varying")]
    [MaxLength(300)]
    public string Author { get; set; } = null!;

    [Required]
    [Column("yearpublished")]
    public int YearPublished { get; set; }

    [Column("tocxml", TypeName = "xml")]
    public string? TocXml { get; set; }

    [Column("createdat")]
    public DateTime CreatedAt { get; set; }

    [Column("updatedat")]
    public DateTime UpdatedAt { get; set; }
}