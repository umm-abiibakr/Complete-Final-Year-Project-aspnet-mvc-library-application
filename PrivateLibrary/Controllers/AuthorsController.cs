using Microsoft.AspNetCore.Mvc;
using PrivateLibrary.Data;

namespace PrivateLibrary.Controllers
{
    public class AuthorsController : Controller
    { 
        private readonly AppDbContext _context;
        public AuthorsController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var data = _context.Authors.ToList();
            return View(data);
        }
    }
}
