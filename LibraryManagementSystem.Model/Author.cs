namespace LibraryManagementSystem.Model
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public DateTime? BirthDate { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string Biography { get; set; }
        public List<BookAuthor>? BookAuthors { get; set; } = new List<BookAuthor>();
    }
}
