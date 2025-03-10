using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data;
using PrivateLibrary.Data.Services;
using PrivateLibrary.Models;

namespace PrivateLibrary.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublishersService _service;
        public PublishersController(IPublishersService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var allPublishers = await _service.GetAllAsync();
            return View(allPublishers);
        }

        //Get publisher details
        public async Task<IActionResult> Details (int publisherId)
        {
            var publisherDetails = await _service.GetByIdAsync(publisherId);
            if (publisherDetails == null) return View("Not Found");
            return View(publisherDetails);
        }

        //create producers
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Location,LogoUrl")] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage); 
                }
                return View(publisher); 
            }

            await _service.AddAsync(publisher);
            return RedirectToAction(nameof(Index));
        }



    }
}
