using PrivateLibrary.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class Publisher : IEntityBase
    {
        [Key]
        public int PublisherId { get; set; }

        [Display(Name = "Logo")]
        [Required(ErrorMessage = "Publisher Logo is required")]
        public  string LogoUrl { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Publisher Name is required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Full Name must be between 5 and 50 characters")]
        public string Name { get; set; }

        [Display(Name = "Location")]
        [Required(ErrorMessage = "Publisher Location is required")]
        public string Location { get; set; }

        //Relationships
        public List<Book>? Books { get; set; }
    }
}
