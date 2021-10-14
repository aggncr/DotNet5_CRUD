using DotNet5_CRUD.Models.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;

namespace DotNet5_CRUD.Models.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("DotNet5_CRUD");
        }
    }
}
