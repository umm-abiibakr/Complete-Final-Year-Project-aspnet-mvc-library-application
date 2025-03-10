using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data.Base;
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

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            var bookDetails = await _context.Books
                .Include(p => p.Publisher)
                .Include(ba => ba.Books_Authors).ThenInclude(a => a.Author)
                .FirstOrDefaultAsync(n => n.BookId == bookId);

            return bookDetails;
        }
    }
}
