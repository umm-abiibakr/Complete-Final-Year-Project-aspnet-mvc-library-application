using PrivateLibrary.Models;

namespace PrivateLibrary.Data.Services
{
    public interface IAuthorsService
    {
        Task<IEnumerable<Author>> GetAll();
        Author GetById(int id);

        //add data to the database
        Task Add(Author author);

        //update database data
        Author Update(int id, Author newAuthor);

        //Delete
        void Delete(int id);
    }
}
