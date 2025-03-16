using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class LoanCartItem
    {
        [Key]
        public int LoanItemCartId { get; set; }

        public Book Book { get; set; }
        public int Quantity { get; set; }

        public string LoanCartId { get; set; }
    }
}
