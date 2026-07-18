using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAPI.Core.Entities
{
    public class ComplaintMaster
    {
        [Key]
        public int ComplaintId { get; set; }

        [StringLength(200)]
        public string? Name { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(100)]
        public string? District { get; set; }

        [StringLength(100)]
        public string? Tehsil { get; set; }

        [StringLength(500)]
        public string? Address { get; set; }

        [StringLength(200)]
        public string? ConcernedDepartment { get; set; }

        [StringLength(50)]
        public string? ComplaintDate { get; set; }

        [Column(TypeName = "text")]
        public string? Details { get; set; }

        [StringLength(1000)]
        public string? VideoPath { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(50)]
        public string? ModifiedBy { get; set; }
    }
}
