using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fujitsu.CvQc.Data.Entities
{
    public class ProjectConfig
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public int ImportMode { get; set; } = 0;
        public string TemplatePath { get; set; } = "";
        public string InputPath { get; set; } = "";
        public string OutputPath { get; set; } = "";
        public string OutputSuffix { get; set; } = "";
        public string Rules { get; set; } = "";

        [ForeignKey(nameof(Project))]
        public Guid ProjectId { get; set; }
        public Project Project { get; init; } = null!;
    }
}
