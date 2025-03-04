using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class Publisher
    {
        [Key]
        public int PublisherId { get; set; }
        public  string LogoUrl { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        //Relationships
        public List<Book> Books { get; set; }
    }
}
