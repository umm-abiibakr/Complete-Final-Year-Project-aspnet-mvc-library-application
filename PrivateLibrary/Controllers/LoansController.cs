using Microsoft.AspNetCore.Mvc;
using PrivateLibrary.Data.Cart;
using PrivateLibrary.Data.Services;
using PrivateLibrary.Data.ViewModels;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class LoansController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly LoanCart _loanCart;
        public LoansController(IBooksService booksServic, LoanCart loanCart)
        {
            //inject services
            _booksService = booksServic;
            _loanCart = loanCart ?? throw new ArgumentNullException(nameof(loanCart));
        }

        //get and display all list of loan cart items
        public IActionResult LoanCart()
        {
            var items = _loanCart.GetLoanCartItems();

            // If cart is empty, set LoanCartId to empty and DueDate to today's date
            var response = new LoanCartVM
            {
                Title = "Loan Cart",
                LoanCart = items.Any() ? _loanCart : null, // Set to null when empty
                LoanCartItems = items ?? new List<LoanCartItem>(),
                DueDate = items.Any() ? DateTime.Now.AddDays(14) : DateTime.Now // Use today's date when cart is empty
            };

            return View(response);
        }

        public async Task<RedirectToActionResult> AddToLoanCart(int loanCartId)
        {
            var item = await _booksService.GetBookByIdAsync(loanCartId);

            if (item != null)
            {
                _loanCart.AddItemToCart(item);
            }

            return RedirectToAction(nameof(Index)); // Redirect to LoanCart page
        }



    }
}
