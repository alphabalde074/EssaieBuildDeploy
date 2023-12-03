using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fujitsu.CvQc.Data.Entities
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Message { get; set; } = "";
        [Required]
        public Guid JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        public Job Job { get; init; } = null!;
    }
}
