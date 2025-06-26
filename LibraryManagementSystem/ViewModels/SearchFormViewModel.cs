namespace LibraryManagementSystem.ViewModels
{
    public class SearchFormViewModel
    {
        public string Value { get; set; }
        public string Option { get; set; }
        public List<SearchResultViewModel>? Results { get; set; } = new List<SearchResultViewModel>();
    }
}
