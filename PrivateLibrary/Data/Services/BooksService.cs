using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data.Base;
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

    }
}

