using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Models;

namespace PrivateLibrary.Data.Cart
{
    public class LoanCart
    {
        public AppDbContext _context { get; set; }
        public  string LoanCartId { get; set; }
        public List<LoanCartItem> LoanCartItems { get; set; }
        public LoanCart(AppDbContext context) 
        {
            _context = context;
        }

        //gat loan cart
        public static LoanCart GetLoanCart(IServiceProvider services) 
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();

            string loanCartId = session.GetString("LoanCartId") ?? Guid.NewGuid().ToString();
            session.SetString("LoanCartId", loanCartId);

            return new LoanCart(context)
            {
                LoanCartId = loanCartId
            };
        
        }
        public void AddItemToCart(Book book)
        {
            var loanCartItem = _context.LoanCartItems.FirstOrDefault(n => n.Book.BookId == book.BookId && n.LoanCartId == LoanCartId);

            if(loanCartItem == null)
            {
                loanCartItem = new LoanCartItem()
                {
                    LoanCartId = LoanCartId,
                    Book = book,
                    Quantity = 1
                };

                _context.LoanCartItems.Add(loanCartItem);
            }
            else
            {
                // Prevent adding the same book twice
                throw new InvalidOperationException("This book is already in your cart.");
            }
            _context.SaveChanges();
        }

        //remove from cart
        public void RemoveItemFromCart(Book book)
        {
            var loanCartItem = _context.LoanCartItems.FirstOrDefault(n => n.Book.BookId == book.BookId && n.LoanCartId == LoanCartId);

            if (loanCartItem != null)
            {
                //check for quantity
                if(loanCartItem.Quantity == 1)
                {
                    _context.LoanCartItems.Remove(loanCartItem);
                }
                _context.SaveChanges();
            }
        }

        //get all cart items
        public List<LoanCartItem> GetLoanCartItems()
        {
            return LoanCartItems ?? (LoanCartItems = _context.LoanCartItems
                .Where(n => n.LoanCartId == LoanCartId)
                .Include(n => n.Book)
                .ToList());
        }

        //calculate return date
        public DateTime GetDueReturnDate(int loanItemId)
        {
            var loanItem = _context.LoanItems
                .FirstOrDefault(b => b.LoanItemId == loanItemId);

            if (loanItem == null)
                throw new Exception("Loan record not found.");

            return loanItem.DueDate.AddDays(14);
        }
    }
}
