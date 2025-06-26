namespace LibraryManagementSystem.Model
{
    public class Loan
    {
        public int LoanId { get; set; }
        public int ReaderId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public Reader? Reader { get; set; }
        public List<LoanDetail>? LoanDetails { get; set; } = new List<LoanDetail>();
    }
}
