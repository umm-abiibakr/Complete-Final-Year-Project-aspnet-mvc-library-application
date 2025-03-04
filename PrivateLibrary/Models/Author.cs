using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }

        //Relationships
        public  List<Book_Author> Books_Authors { get; set; }
    }
}
