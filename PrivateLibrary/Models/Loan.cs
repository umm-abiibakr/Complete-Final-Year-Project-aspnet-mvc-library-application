using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models
{
    public class Loan
    {
        [Key]
        public int LoanId { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }

        public List<LoanItem> LoanItems { get; set; }
    }
}
