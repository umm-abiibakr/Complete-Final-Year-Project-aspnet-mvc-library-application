using PrivateLibrary.Data.Base;
using PrivateLibrary.Data.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateLibrary.Models
{
    public class Book : IEntityBase
    {
        [Key]
        public int BookId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public  bool Status { get; set; }
        public BookCategory BookCategory { get; set; }

        //Relationships
        public List<Book_Author> Books_Authors { get; set; }
        //Publisher
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }


    }
}
