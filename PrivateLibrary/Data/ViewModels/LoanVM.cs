using System;
using System.Collections.Generic;

namespace PrivateLibrary.Data.ViewModels
{
    public class LoanVM
    {
        public List<LoanCartItemVM> LoanCartItems { get; set; }
    }

    public class LoanCartItemVM
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public DateTime DueDate { get; set; }
    }
}