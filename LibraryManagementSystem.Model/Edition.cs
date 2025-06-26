using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Model
{
    public class Edition
    {
        public int EditionId { get; set; }
        public int BookId {  get; set; }
        public string ISBN { get; set; }
        public int EditorialId { get; set; }
        public DateTime EditionDate { get; set; }
        public Book? Book { get; set; }
        public Editorial? Editorial { get; set; }
    }
}
