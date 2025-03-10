using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data;
using PrivateLibrary.Data.Services;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBooksService _service;
        public BooksController(IBooksService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allBooks = await _service.GetAllAsync();
            return View(allBooks);
        }

        //get books details
        public async Task<IActionResult> Details(int bookId)
        {
            var bookDetail = await _service.GetBookByIdAsync(bookId);
            return View(bookDetail);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.Description = "Welcome to my Library";
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Create([Bind("Name,Location,LogoUrl")] Publisher publisher)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        //        {
        //            Console.WriteLine(error.ErrorMessage);
        //        }
        //        return View(publisher);
        //    }

        //    await _service.AddAsync(publisher);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
