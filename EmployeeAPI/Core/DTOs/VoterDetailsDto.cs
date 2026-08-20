namespace EmployeeAPI.Core.DTOs
{
    public class VoterDetailsDto
    {
        public int Id { get; set; }
        public int AreaNumber { get; set; }
        public int PartNumber { get; set; }
        public string? IsMuslim { get; set; }
        public string? CleanEnglishName { get; set; }
        public string? CleanEnglishRelativeName { get; set; }
    }
}
