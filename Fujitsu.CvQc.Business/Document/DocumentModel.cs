using System.ComponentModel.DataAnnotations;

namespace Fujitsu.CvQc.Business
{
    public class DocumentModel
    {
        [Required]
        [StringLength(250, MinimumLength = 1)]
        public string FileName { get; set; } = "";

        [Required]
        public DocumentMapping? DocumentMap { get; set; } = null;

        public DateTime Importation { get; set; } = DateTime.Now;

        public string ProjectId { get; set; } = "";
    }
}