using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace EmployeeAPI.Core.Entities
{

    public class ActMaster
    {
        [Key]
        public int ActId { get; set; }

        [StringLength(500)]
        public string? ActName { get; set; }

        [StringLength(200)]
        public string? Alias { get; set; }

        public DateTime? DateOfEffect { get; set; }

        [Column(TypeName = "text")]
        public string? ActDetails { get; set; }

        [StringLength(500)]
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile? ActImage { get; set; }

        public int? LegalCategoryId { get; set; }

       // public LegalCategoryMaster? LegalCategoryMaster { get; set; }

        public DateTime? CreatedOn { get; set; }

        [StringLength(50)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [StringLength(50)]
        public string? ModifiedBy { get; set; }
    }
    
}
