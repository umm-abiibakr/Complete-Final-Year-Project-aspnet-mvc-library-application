using PrivateLibrary.Data.Base;
using PrivateLibrary.Models;

namespace PrivateLibrary.Data.Services
{
    public interface IBooksService : IEntityBaseRepository<Book>
    {
        Task<Book> GetBookByIdAsync(int bookId);
    }
}
