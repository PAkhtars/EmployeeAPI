using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAPI.Core.Entities
{
    public class ActDetails
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Section { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Offence { get; set; }

        [StringLength(500)]
        public string? Punishment { get; set; }

        [StringLength(500)]
        public string? CognizableOrNon_Cognizable { get; set; }

        [StringLength(150)]
        public string? BailableOrNon_Bailable { get; set; }

        [StringLength(500)]
        public string? TrialCourt { get; set; }

        public int? ActName { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(50)]
        public string? ModifeidBy { get; set; }
    }
}
