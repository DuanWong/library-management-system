namespace LibraryManagementSystem.Model
{
    public class Rating
    {
        public int RatingId { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public short Rate { get; set; }
        public string Comment { get; set; }
        public Book? Book { get; set; }
        public Reader? Reader { get; set; }
    }
}
