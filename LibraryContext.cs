using Microsoft.EntityFrameworkCore;

namespace LibrarySystem
{
    public class LibraryContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Connect to the LocalDB instance with your LibraryDB database
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=LibraryDB;Trusted_Connection=True;");
        }
    }
}

