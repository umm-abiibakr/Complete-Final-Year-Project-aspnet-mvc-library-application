using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data.Base;
using PrivateLibrary.Data.Enums;
using PrivateLibrary.Data.ViewModels;
using PrivateLibrary.Models;

namespace PrivateLibrary.Data.Services
{
    public class BooksService : EntityBaseRepository<Book>, IBooksService
    {
        private readonly AppDbContext _context;
        public BooksService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddNewBookAsync(NewBookVM newBook)
        {
            var book = new Book
            {
                Title = newBook.Title,
                Description = newBook.Description,
                ImageUrl = newBook.ImageUrl,
                Status = newBook.Status,
                BookCategory = newBook.BookCategory,
                Language = newBook.Language,
                PublisherId = newBook.PublisherId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            // Save author relationships
            foreach (var authorId in newBook.AuthorIds)
            {
                _context.Books_Authors.Add(new Book_Author
                {
                    BookId = book.BookId,
                    AuthorId = authorId
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            var bookDetails = await _context.Books
                .Include(p => p.Publisher)
                .Include(ba => ba.Books_Authors).ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(n => n.BookId == bookId);

            return bookDetails;
        }

        public async Task<NewBookDropdownsVM> GetNewBookDropdownsValues()
        {
            return new NewBookDropdownsVM
            {
                Authors = await _context.Authors.OrderBy(a => a.FullName).ToListAsync(),
                Publishers = await _context.Publishers.OrderBy(p => p.Name).ToListAsync()
            };
        }

        public async Task UpdateBookAsync(NewBookVM newBook)
        {
            // Load the book including its authors to maintain EF Core tracking
            var dbBook = await _context.Books
                .Include(b => b.Books_Authors)
                .FirstOrDefaultAsync(n => n.BookId == newBook.BookId);

            if (dbBook == null) return; // Stop execution if the book is not found

            // Update book properties
            dbBook.Title = newBook.Title;
            dbBook.Description = newBook.Description;
            dbBook.ImageUrl = newBook.ImageUrl;
            dbBook.Status = newBook.Status;
            dbBook.BookCategory = newBook.BookCategory;
            dbBook.Language = newBook.Language;
            dbBook.PublisherId = newBook.PublisherId;

            // Remove existing authors
            _context.Books_Authors.RemoveRange(dbBook.Books_Authors);

            // Add the new authors
            dbBook.Books_Authors = newBook.AuthorIds
                .Select(authorId => new Book_Author { BookId = dbBook.BookId, AuthorId = authorId })
                .ToList();

            // Save changes once
            await _context.SaveChangesAsync();
        }


        public async Task DeleteBookAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book != null)
            {
                // Remove related Books_Authors entries
                var bookAuthors = _context.Books_Authors.Where(ba => ba.BookId == bookId);
                _context.Books_Authors.RemoveRange(bookAuthors);

                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }

        }

    }
}

