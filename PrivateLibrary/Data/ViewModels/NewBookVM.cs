using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using PrivateLibrary.Data.Enums;

namespace PrivateLibrary.Models
{
    public class NewBookVM
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Image path is required")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Book status is required")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Book category is required")]
        public BookCategory BookCategory { get; set; }

        [Required(ErrorMessage = "Language is required")]
        public Language Language { get; set; }

        [Required(ErrorMessage = "Book author(s) is required")]
        public List<int> AuthorIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Book publisher is required")]
        public int PublisherId { get; set; }

        // Dropdown lists (Populated in controller)
        public IEnumerable<SelectListItem> PublisherList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AuthorList { get; set; } = new List<SelectListItem>();
    }
}
