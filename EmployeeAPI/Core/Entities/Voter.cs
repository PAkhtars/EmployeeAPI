using System.ComponentModel.DataAnnotations;

namespace EmployeeAPI.Core.Entities
{
    public class Voter
    {
        [Key]
        public int Id { get; set; }

        public int SerialNo { get; set; }

        [StringLength(100)]
        public string EpicNo { get; set; } = string.Empty;

        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? EnglishName { get; set; }

        [StringLength(200)]
        public string? RelativeName { get; set; }

        [StringLength(200)]
        public string? EnglishRelativeName { get; set; }

        [StringLength(200)]
        public string? HouseNo { get; set; }

        public int? Age { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }

        [StringLength(50)]
        public string? PartNumber { get; set; }

        [StringLength(200)]
        public string? AreaName { get; set; }

        [StringLength(50)]
        public string? AreaNumber { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }
    }
}
