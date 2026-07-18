namespace EmployeeAPI.Core.DTOs
{
    public class ComplaintMasterCreateDto
    {
        public string? Name { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? District { get; set; }
        public string? Tehsil { get; set; }
        public string? Address { get; set; }
        public string? ConcernedDepartment { get; set; }
        public string? ComplaintDate { get; set; }
        public string? Details { get; set; }
        public string? VideoPath { get; set; }
    }
}
