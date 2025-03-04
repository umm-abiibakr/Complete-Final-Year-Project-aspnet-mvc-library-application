using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Models;

namespace PrivateLibrary.Data
{
    public class AppDbContext : DbContext //translator between c# models
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //define identifiers
            modelBuilder.Entity<Book_Author>().HasKey(ba => new
            {
                ba.BookId,
                ba.AuthorId
            });

            //joining models
            modelBuilder.Entity<Book_Author>().HasOne(b => b.Book).WithMany(ba => ba.Books_Authors)
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<Book_Author>().HasOne(b => b.Author).WithMany(ba => ba.Books_Authors)
                .HasForeignKey(b => b.AuthorId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Author>  Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Book_Author> Books_Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}
