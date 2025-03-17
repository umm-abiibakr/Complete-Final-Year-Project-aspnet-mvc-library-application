using System;
using System.Collections.Generic;

namespace PrivateLibrary.Data.ViewModels
{
    public class ReturnLoanVM
    {
        public int LoanId { get; set; }
        public string BorrowerEmail { get; set; }
        public List<LoanItemVM> LoanItems { get; set; }
    }

    public class LoanItemVM
    {
        public string BookTitle { get; set; }
        public int Quantity { get; set; }
        public DateTime DueDate { get; set; }
    }
}