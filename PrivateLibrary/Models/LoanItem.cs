using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrivateLibrary.Models
{
    public class LoanItem
    {
        [Key]
        public int LoanItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        public int LoanId { get; set; }
        [ForeignKey("LoanId")]
        public Loan loan { get; set; }
    }

}

