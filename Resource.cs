using System;

namespace LibrarySystem
{
    public class Resource
    {
        public int Id { get; set; } // Primary Key
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime? DueDate { get; set; }
    }
}
