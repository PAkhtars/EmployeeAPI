using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAPI.Core.Entities
{
    public class LegalCategoryMaster
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Alias { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? CategoryIconPath { get; set; }

        [StringLength(200)]
        public string? IconClass { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }
    }
}
