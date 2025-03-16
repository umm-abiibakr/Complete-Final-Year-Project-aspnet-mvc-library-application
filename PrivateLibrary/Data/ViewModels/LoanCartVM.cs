using PrivateLibrary.Data.Cart;
using PrivateLibrary.Models;

namespace PrivateLibrary.Data.ViewModels
{
    public class LoanCartVM
    {
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public LoanCart LoanCart { get; set; }
        public List<LoanCartItem> LoanCartItems { get; set; }

    }
}
