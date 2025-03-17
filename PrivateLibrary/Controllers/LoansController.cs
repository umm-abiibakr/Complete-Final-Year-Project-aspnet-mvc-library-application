using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data;
using PrivateLibrary.Data.Cart;
using PrivateLibrary.Data.Services;
using PrivateLibrary.Data.ViewModels;
using PrivateLibrary.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateLibrary.Controllers
{
    public class LoansController : Controller
    {
        private readonly IBooksService _booksService;
        private readonly LoanCart _loanCart;
        private readonly AppDbContext _context;

        public LoansController(IBooksService booksService, LoanCart loanCart, AppDbContext context)
        {
            _booksService = booksService ?? throw new ArgumentNullException(nameof(booksService));
            _loanCart = loanCart ?? throw new ArgumentNullException(nameof(loanCart));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Loans/LoanCart
        public IActionResult LoanCart()
        {
            var items = _loanCart.GetLoanCartItems() ?? new List<LoanCartItem>();
            var response = new LoanCartVM
            {
                Title = "Loan Cart",
                LoanCart = items.Any() ? _loanCart : null,
                LoanCartItems = items,
                DueDate = items.Any() ? DateTime.Now.AddDays(14) : DateTime.Now
            };
            return View(response);
        }

        // POST: Loans/AddToLoanCart
        public async Task<RedirectToActionResult> AddToLoanCart(int bookId)
        {
            try
            {
                var item = await _booksService.GetBookByIdAsync(bookId);
                if (item == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {bookId} to add to loan cart.";
                    return RedirectToAction(nameof(LoanCart));
                }
                if (!item.Status)
                {
                    TempData["ErrorMessage"] = $"The book '{item.Title}' is not available to add to the loan cart.";
                    return RedirectToAction(nameof(LoanCart));
                }
                _loanCart.AddItemToCart(item);
                TempData["SuccessMessage"] = $"Added '{item.Title}' to the loan cart.";
                return RedirectToAction(nameof(LoanCart));
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(LoanCart));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the book to the loan cart.";
                return RedirectToAction(nameof(LoanCart));
            }
        }

        // POST: Loans/RemoveFromLoanCart
        public async Task<RedirectToActionResult> RemoveFromLoanCart(int bookId)
        {
            try
            {
                var book = await _booksService.GetBookByIdAsync(bookId);
                if (book == null)
                {
                    TempData["ErrorMessage"] = $"No book found with ID {bookId} to remove from loan cart.";
                    return RedirectToAction(nameof(LoanCart));
                }
                _loanCart.RemoveItemFromCart(book);
                TempData["SuccessMessage"] = $"Removed '{book.Title}' from the loan cart.";
                return RedirectToAction(nameof(LoanCart));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while removing the book from the loan cart.";
                return RedirectToAction(nameof(LoanCart));
            }
        }

        // GET: Loans/Create
        public IActionResult Create()
        {
            var items = _loanCart.GetLoanCartItems() ?? new List<LoanCartItem>();
            if (!items.Any())
            {
                TempData["ErrorMessage"] = "The loan cart is empty. Please add books before borrowing.";
                return View("Not Found");
            }

            try
            {
                var loanVM = new LoanVM
                {
                    LoanCartItems = items.Select(i => new LoanCartItemVM
                    {
                        BookId = i.Book.BookId,
                        BookTitle = i.Book.Title,
                        Quantity = i.Quantity,
                        DueDate = DateTime.Now.AddDays(14)
                    }).ToList()
                };
                return View(loanVM);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the borrow form.";
                return View("Error");
            }
        }

        // POST: Loans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LoanVM model)
        {
            if (!ModelState.IsValid)
            {
                // Re-populate the model with current cart items if validation fails
                var items = _loanCart.GetLoanCartItems() ?? new List<LoanCartItem>();
                model.LoanCartItems = items.Select(i => new LoanCartItemVM
                {
                    BookId = i.Book.BookId,
                    BookTitle = i.Book.Title,
                    Quantity = i.Quantity,
                    DueDate = DateTime.Now.AddDays(14)
                }).ToList();
                return View(model);
            }

            try
            {
                // Ensure the cart is initialized with the current session
                var items = _loanCart.GetLoanCartItems() ?? new List<LoanCartItem>();
                if (!items.Any())
                {
                    TempData["ErrorMessage"] = "The loan cart is empty. Please add books before borrowing.";
                    return View("Not Found");
                }

                var loan = new Loan
                {
                    UserId = "DefaultUserId", // Replace with authenticated user ID
                    Email = "user@example.com", // Replace with authenticated user email
                    LoanItems = new List<LoanItem>()
                };

                foreach (var item in items)
                {
                    var book = await _booksService.GetBookByIdAsync(item.Book.BookId);
                    if (book == null || !book.Status)
                    {
                        TempData["ErrorMessage"] = $"The book '{item.Book.Title}' is not available for borrowing.";
                        return View("NotAvailable");
                    }

                    var loanItem = new LoanItem
                    {
                        BookId = book.BookId,
                        Quantity = item.Quantity,
                        DueDate = DateTime.Now.AddDays(14),
                        Book = book
                    };
                    loan.LoanItems.Add(loanItem);
                    book.Status = false;
                }

                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                _loanCart.ClearCart();
                TempData["SuccessMessage"] = "Successfully borrowed the books from the loan cart.";
                return RedirectToAction("Return", new { loanId = loan.LoanId }); // Redirect to Return page
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the borrowing action.";
                return View("Error");
            }
        }

        // GET: Loans/Return/5
        public async Task<IActionResult> Return(int? loanId)
        {
            if (loanId == null || loanId == 0)
            {
                TempData["ErrorMessage"] = "Loan ID was not provided for returning.";
                return View("Not Found");
            }

            try
            {
                var loan = await _context.Loans
                    .Include(l => l.LoanItems)
                    .ThenInclude(li => li.Book)
                    .FirstOrDefaultAsync(l => l.LoanId == loanId);
                if (loan == null)
                {
                    TempData["ErrorMessage"] = $"No loan record found with ID {loanId} to return.";
                    return View("Not Found");
                }

                var allReturned = loan.LoanItems.All(li => li.DueDate < DateTime.Now); // Note: Consider using ReturnDate instead
                if (allReturned)
                {
                    TempData["ErrorMessage"] = "This loan has already been fully returned.";
                    return View("AlreadyReturned");
                }

                var returnViewModel = new ReturnLoanVM
                {
                    LoanId = loan.LoanId,
                    BorrowerEmail = loan.Email,
                    LoanItems = loan.LoanItems.Select(li => new LoanItemVM
                    {
                        BookTitle = li.Book.Title,
                        DueDate = li.DueDate,
                        Quantity = li.Quantity
                    }).ToList()
                };
                return View(returnViewModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the return confirmation.";
                return View("Error");
            }
        }

        // POST: Loans/Return/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return(ReturnLoanVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loan = await _context.Loans
                    .Include(l => l.LoanItems)
                    .ThenInclude(li => li.Book)
                    .FirstOrDefaultAsync(l => l.LoanId == model.LoanId);
                if (loan == null)
                {
                    TempData["ErrorMessage"] = $"No loan record found with ID {model.LoanId} to return.";
                    return View("Not Found");
                }

                var allReturned = loan.LoanItems.All(li => li.DueDate < DateTime.Now); // Note: Consider using ReturnDate instead
                if (allReturned)
                {
                    TempData["ErrorMessage"] = "This loan has already been fully returned.";
                    return View("AlreadyReturned");
                }

                foreach (var loanItem in loan.LoanItems)
                {
                    loanItem.DueDate = DateTime.Now;
                    loanItem.Book.Status = true;
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Successfully returned the books in the loan.";
                return RedirectToAction("Index", "Books");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing the return action.";
                return View("Error");
            }
        }
    }
}