using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeAPI.Core.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(50)]
        public string Department { get; set; } = string.Empty;

        [StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        public DateTime JoinDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]           // ← Best fix
        public decimal Salary { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [StringLength(100)]
        public string State { get; set; } = string.Empty;

        [StringLength(100)]
        public string Country { get; set; } = "India";
    }
}