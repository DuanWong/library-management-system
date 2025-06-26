namespace LibraryManagementSystem.Model
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
