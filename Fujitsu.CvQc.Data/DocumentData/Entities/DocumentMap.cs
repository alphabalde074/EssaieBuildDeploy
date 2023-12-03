using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fujitsu.CvQc.Data.Entities;

public class DocumentMap
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    /*[StringLength(250)]
    public string DocumentFileName { get; set; } = "";*/
    
    [Required]
    public IEnumerable<DocumentMapSection> Sections { get; set; } = new List<DocumentMapSection>();
}

