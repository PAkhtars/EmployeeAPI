using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAPI.Core.Entities
{
    public class ActSectionDtls
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SectionId { get; set; }

        public int ActId { get; set; }

        public int SectionNo { get; set; }

        [StringLength(500)]
        public string? ChapterName { get; set; }

        [StringLength(2000)]
        public string? BareAct { get; set; }

        [Column(TypeName = "text")]
        public string? Meaning { get; set; }

        [Column(TypeName = "text")]
        public string? Objective { get; set; }

        [Column(TypeName = "text")]
        public string? Illustration { get; set; }

        [Column(TypeName = "text")]
        public string? Exception { get; set; }

        public int? CaseStudyId { get; set; }
    }
}
