using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }

        [Display(Name = "Logo")]
        public  string LogoUrl { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        //Relationships
        public List<Book> Books { get; set; }
    }
}
