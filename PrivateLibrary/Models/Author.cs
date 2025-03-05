using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }

        //Relationships
        public  List<Book_Author> Books_Authors { get; set; }
    }
}
