namespace LibraryManagementSystem.Model
{
    public class Editorial
    {
        public int EditorialId { get; set; }
        public string Name { get; set; }
        public ICollection<Edition>? Editions { get; set; }

    }
}
