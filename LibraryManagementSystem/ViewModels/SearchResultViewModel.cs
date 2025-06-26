namespace LibraryManagementSystem.ViewModels
{
    public class SearchResultViewModel
    {
        public int BookId { get; set; }
        public string? Title { get; set; }
        public int SubjectId { get; set; }
        public string? Subject { get; set; }
        public string? Synopsis { get; set; }
        public string? Authors { get; set; }
        public bool Available { get; set; }
        public bool Rate { get; set; }
        public string? Comment { get; set; }
    }
}