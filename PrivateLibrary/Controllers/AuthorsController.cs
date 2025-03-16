using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data;
using PrivateLibrary.Data.Services;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class AuthorsController : Controller
    { 
        private readonly IAuthorsService _service;
        public AuthorsController(IAuthorsService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get request for authors
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName,Bio")] Author author)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); 
                }
                return View(author);
            }

            await _service.AddAsync(author); 
            return RedirectToAction(nameof(Index));
        }

        //get request to get Id
        public async Task<IActionResult> Details(int authorId) 
        {
            // Check if id exists
            var authorDetails = await _service.GetByIdAsync(authorId);

            // Check if details are null
            if (authorDetails == null) return View("Not Found");

            return View(authorDetails);
        }



        // GET: Edit Author
        public async Task<IActionResult> Edit(int authorId)
        {
            var authorDetails = await _service.GetByIdAsync(authorId);

            // Check if details are null
            if (authorDetails == null) return View("Not Found"); 

            return View(authorDetails);
        }


        [HttpPost]
        public async Task<IActionResult> Edit([Bind("AuthorId,FullName,Bio")] Author author)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(author);
            }

            await _service.UpdateAsync(author.AuthorId, author);
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete Author
        public async Task<IActionResult> Delete(int authorId)
        {
            var authorDetails = await _service.GetByIdAsync(authorId);

            // Check if details are null
            if (authorDetails == null) return View("Not Found");

            return View(authorDetails);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int authorId)
        {
            var authorDetails = await _service.GetByIdAsync(authorId);

            // Check if details are null
            if (authorDetails == null) return NotFound();

            await _service.DeleteAsync(authorId);

            return RedirectToAction(nameof(Index));
        }

    }
}
