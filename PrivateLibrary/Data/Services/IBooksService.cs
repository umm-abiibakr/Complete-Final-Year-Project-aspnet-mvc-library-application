using PrivateLibrary.Data.Base;
using PrivateLibrary.Data.ViewModels;
using PrivateLibrary.Models;

namespace PrivateLibrary.Data.Services
{
    public interface IBooksService : IEntityBaseRepository<Book>
    {
        Task<Book> GetBookByIdAsync(int bookId);
        Task<NewBookDropdownsVM> GetNewBookDropdownsValues();

        Task AddNewBookAsync(NewBookVM newBook);

        Task UpdateBookAsync(NewBookVM newBook);
        Task DeleteBookAsync(int bookId);

    }
}
