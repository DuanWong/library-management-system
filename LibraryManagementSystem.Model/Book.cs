using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Model
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public int SubjectId { get; set; }
        public string Synopsis { get; set; }
        public string Photo { get; set; }
        public Subject? Subject { get; set; }
        public List<Edition> Editions { get; set; }
        public ICollection<BookAuthor>? BookAuthors { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<LoanDetail>? LoanDetails { get; set; }

        [NotMapped]
        public string Authors
        {
            get
            {
                if (BookAuthors == null || BookAuthors.Count == 0)
                    return string.Empty;

                string authors = string.Join(", ",
                    BookAuthors
                        .Where(ba => ba.Author != null)
                        .Select(ba => $"{ba?.Author?.FirstName} {ba?.Author?.LastName}")
                );

                return authors;
            }
        }
    }
}
