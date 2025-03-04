using PrivateLibrary.Data;
using PrivateLibrary.Data.Enums;
using PrivateLibrary.Models;

public class AppDbInitializer
{
    public static void Seed(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            // Create reference to the app db context file
            var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

            // Check if database exists
            context.Database.EnsureCreated();

            // Publishers
            if (!context.Publishers.Any())
            {
                context.AddRange(new List<Publisher>()
                {
                    new Publisher
                    {
                        Name = "Authentic Statements",
                        LogoUrl = "https://cdn11.bigcommerce.com/s-qrk4pkvixl/product_images/uploaded_images/220px-darussalam-publishers-logo.jpeg",
                        Location = "America"
                    },
                    new Publisher
                    {
                        Name = "Sunnah Statements",
                        LogoUrl = "https://cdn11.bigcommerce.com/s-qrk4pkvixl/product_images/uploaded_images/220px-darussalam-publishers-logo.jpeg",
                        Location = "Africa"
                    },
                    new Publisher
                    {
                        Name = "Statements",
                        LogoUrl = "https://cdn11.bigcommerce.com/s-qrk4pkvixl/product_images/uploaded_images/220px-darussalam-publishers-logo.jpeg",
                        Location = "Asia"
                    }
                });
                context.SaveChanges();
            }

            // Authors
            if (!context.Authors.Any())
            {
                context.AddRange(new List<Author>()
                {
                    new Author
                    {
                        FullName = "Ibnul Qayyim",
                        Bio = "Author's brief bio goes in here"
                    },
                    new Author
                    {
                        FullName = "Ibnul Qayyim",
                        Bio = "Author's brief bio goes in here"
                    },
                    new Author
                    {
                        FullName = "Ibnul Qayyim",
                        Bio = "Author's brief bio goes in here"
                    }
                });
                context.SaveChanges();
            }

            // Books
            if (!context.Books.Any())
            {
                context.AddRange(new List<Book>()
                {
                    new Book
                    {
                        Title = "The Description Of Paradise",
                        Description = "An explanation of one of the most monumental works of poetry ...",
                        ImageUrl = "url goes here",
                        Status = true,
                        PublisherId = 2,
                        BookCategory = BookCategory.Creed
                    },
                    new Book
                    {
                        Title = "The Description Of Paradise",
                        Description = "An explanation of one of the most monumental works of poetry ...",
                        ImageUrl = "url goes here",
                        Status = false,
                        PublisherId = 2,
                        BookCategory = BookCategory.Creed
                    },
                    new Book
                    {
                        Title = "The Description Of Paradise",
                        Description = "An explanation of one of the most monumental works of poetry ...",
                        ImageUrl = "url goes here",
                        Status = true,
                        PublisherId = 2,
                        BookCategory = BookCategory.Creed
                    }
                });
                context.SaveChanges(); // Ensure books are saved before adding relationships
            }

            // Books_Authors relationships
            if (!context.Books_Authors.Any())
            {
                context.Books_Authors.AddRange(new List<Book_Author>()
                {
                    new Book_Author()
                    {
                        AuthorId = 1,
                        BookId = 1
                    },
                    new Book_Author()
                    {
                        AuthorId = 1,
                        BookId = 2
                    },
                    new Book_Author()
                    {
                        AuthorId = 1,
                        BookId = 3
                    }
                });

                context.SaveChanges();
            }
        }
    }
}
