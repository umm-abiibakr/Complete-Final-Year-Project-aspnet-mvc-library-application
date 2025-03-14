using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data;
using PrivateLibrary.Data.Services;
using PrivateLibrary.Models;
using System.Collections.Generic;

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

        public async Task<IActionResult> Filter(string searchString)
        {
            var allBooks = await _service.GetAllAsync();

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResult = allBooks
                .Where(n => n.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                        n.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToList();
                return View("Index", filteredResult);
            }
            return View("Index", allBooks);
        }

        //get books details
        public async Task<IActionResult> Details(int bookId)
        {
            var bookDetail = await _service.GetBookByIdAsync(bookId);
            return View(bookDetail);
        }

        // GET: Books/Create
        public async Task<IActionResult> Create()
        {
            var bookDropdownsData = await _service.GetNewBookDropdownsValues();

            if (!bookDropdownsData.Publishers.Any() || !bookDropdownsData.Authors.Any())
            {
                ViewBag.ErrorMessage = "No authors or publishers available. Please add them first.";
                return View();
            }

            var model = new NewBookVM
            {
                PublisherList = bookDropdownsData.Publishers
                                  .Select(p => new SelectListItem { Value = p.PublisherId.ToString(), Text = p.Name }),
                AuthorList = bookDropdownsData.Authors
                                  .Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.FullName })
            };

            return View(model);
        }

        // POST: Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewBookVM book)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns when validation fails
                var bookDropdownsData = await _service.GetNewBookDropdownsValues();
                book.PublisherList = bookDropdownsData.Publishers
                    .Select(p => new SelectListItem { Value = p.PublisherId.ToString(), Text = p.Name });
                book.AuthorList = bookDropdownsData.Authors
                    .Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.FullName });

                return View(book);
            }

            await _service.AddNewBookAsync(book);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int bookId)
        {
            var bookDetails = await _service.GetBookByIdAsync(bookId);
            if (bookDetails == null) return View("Not Found");

            var bookDropdownsData = await _service.GetNewBookDropdownsValues();

            if (!bookDropdownsData.Publishers.Any() || !bookDropdownsData.Authors.Any())
            {
                ViewBag.ErrorMessage = "No authors or publishers available. Please add them first.";
                return View();
            }

            var response = new NewBookVM()
            {
                BookId = bookDetails.BookId,
                Title = bookDetails.Title,
                Description = bookDetails.Description,
                Status = bookDetails.Status,
                Language = bookDetails.Language,
                ImageUrl = bookDetails.ImageUrl,
                BookCategory = bookDetails.BookCategory,
                PublisherId = bookDetails.PublisherId,
                AuthorIds = bookDetails.Books_Authors.Select(n => n.AuthorId).ToList(),

                // Populate the dropdowns 
                PublisherList = bookDropdownsData.Publishers
                    .Select(p => new SelectListItem { Value = p.PublisherId.ToString(), Text = p.Name }),
                AuthorList = bookDropdownsData.Authors
                    .Select(a => new SelectListItem { Value = a.AuthorId.ToString(), Text = a.FullName })
            };

            return View(response);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int bookId, NewBookVM book)
        {
            if (bookId != book.BookId) return View("NotFound");

            if (!ModelState.IsValid)
            {
                //Repopulate dropdown lists in case of validation errors
                var bookDropdownsData = await _service.GetNewBookDropdownsValues();
                ViewBag.Publishers = new SelectList(bookDropdownsData.Publishers, "Id", "Name");
                ViewBag.Authors = new SelectList(bookDropdownsData.Authors, "Id", "FullName");

                return View(book);
            }

            // Ensure book exists before updating
            var existingBook = await _service.GetBookByIdAsync(book.BookId);
            if (existingBook == null) return View("Not Found");

            // Update the book
            await _service.UpdateBookAsync(book);

            return RedirectToAction(nameof(Index));
        }

    }
}
