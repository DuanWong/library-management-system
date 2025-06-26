using LibraryManagementSystem.Model;

namespace LibraryManagementSystem.Common.DTOs
{
    public class LoansPerReaderResultDTO
    {
        public int ReaderId { get; set; }
        public string ReaderFullName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public List<Loan> Loans { get; set; } = new List<Loan>();
    }
}
