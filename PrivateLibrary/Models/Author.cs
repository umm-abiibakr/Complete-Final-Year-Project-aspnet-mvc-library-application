using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class Author
    {
        [Key]
        public int AuthorId { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage ="Full Name must be between 5 and 50")]
        public required string FullName { get; set; }

        [Display(Name = "Bio")]
        [Required(ErrorMessage = "Biography is required")]
        public required string Bio { get; set; }

        //Relationships
        public  List<Book_Author>? Books_Authors { get; set; }
    }
}
