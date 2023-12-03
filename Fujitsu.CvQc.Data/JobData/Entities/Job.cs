using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fujitsu.CvQc.Data.Entities
{
    public class Job
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Mode { get; set; } = "";

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? CompletionDate { get; set; } = null;

        [Required]
        public string Status { get; set; } = "";

        [Required]
        public Guid ProjectId { get; set; }

        public Project Project { get; init; } = null!;
        public List<Log> Logs { get; } = new();
    }
}
