using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Model
{
    public class Reader
    {
        public int ReaderId { get; set; }
        public string? CoreId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }

        [RegularExpression(@"^(\+1\s?)?(\(?\d{3}\)?[\s\-]?)\d{3}[\s\-]?\d{4}$",
            ErrorMessage = "Enter a valid phone number like (204) 999 5475, +1 (204) 999 5475, or 204-999-5475")]
        public string? Phone { get; set; }

        public DateTime? BirthDate { get; set; }
        public ICollection<Loan>? Loans { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
    }
}
