namespace LibraryManagementSystem.Model
{
    public class LoanDetail
    {
        public int LoanDetailId { get; set; }
        public int LoanId { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }
        public Loan? Loan { get; set; }
    }
}
