using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fujitsu.CvQc.Data.Entities
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string FileName { get; set; } = "";

        [Required]
        public DocumentMap? DocumentMap { get; set; } = null;

        public DateTime Importation { get; set; } = DateTime.Now;

        [Required]
        public Guid ProjectId { get; set; }

        public Project Project { get; init; } = null!;
    }
}