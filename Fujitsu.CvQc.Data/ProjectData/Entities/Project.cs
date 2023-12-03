using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fujitsu.CvQc.Data.Entities;
public class Project
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ProjectConfig? Config { get; set; } = null;

    public List<Job> Jobs { get; } = new();
}
