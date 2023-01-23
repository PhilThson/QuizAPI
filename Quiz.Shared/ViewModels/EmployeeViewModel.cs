using Quiz.Shared.DTOs;
using Quiz.Shared.DTOs.Read;

namespace Quiz.Shared.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PersonalNumber { get; set; }
        public decimal Salary { get; set; }
        public int? DaysOfLeave { get; set; }
        public double? HourlyRate { get; set; }
        public double? Overtime { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public JobDto? Job { get; set; }
        public PositionDto? Position { get; set; }
        public DateTime? DateOfEmployment { get; set; }
        public DateTime? EmploymentEndDate { get; set; }
        public List<AddressDto> Addresses { get; set; }
    }
}
