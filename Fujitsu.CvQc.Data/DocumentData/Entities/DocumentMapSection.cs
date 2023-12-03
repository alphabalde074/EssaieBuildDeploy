using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fujitsu.CvQc.Data.Entities;

public class DocumentMapSection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Match { get; set; } = "";

    public string Target { get; set; } = "";

    public int ExportationMode { get; set; }

    public IEnumerable<SectionParagraph> Paragraphs { get; set; } = new List<SectionParagraph>();
}

