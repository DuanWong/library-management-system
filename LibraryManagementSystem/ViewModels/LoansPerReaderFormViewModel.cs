namespace LibraryManagementSystem.ViewModels
{
    public class LoansPerReaderFormViewModel
    {
        public string? ReaderFullName { get; set; }
        public string? Email { get; set; }
        public List<LoansPerReaderResultViewModel>? Results { get; set; } = new List<LoansPerReaderResultViewModel>();
    }
}
