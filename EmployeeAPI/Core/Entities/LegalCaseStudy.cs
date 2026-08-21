using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAPI.Core.Entities
{
    public class LegalCaseStudy
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string CaseTitleAndCitation { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(max)")]
        public string? FactsOfTheCase { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? ProceduralHistory { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? LegalIssues { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? ArgumentsOfTheParties { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? RelevantLaw { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? CourtsAnalysis { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Judgment_Holding { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? CriticalAnalysis { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? ImpactOfTheJudgment { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Conclusion { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? References { get; set; }

        [StringLength(500)]
        public string? CaseName { get; set; }

        [StringLength(1000)]
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? CaseImage { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }
    }
}
