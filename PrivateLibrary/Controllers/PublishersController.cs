using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.Data;
using PrivateLibrary.Data.Services;
using PrivateLibrary.Models;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Details(int id)
        {
            var publisherDetails = await _service.GetByIdAsync(id);
            if (publisherDetails == null) return View("Not Found");

            return View(publisherDetails);
        }


        //create publisher
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

        //edit publisher
        public async Task<IActionResult> Edit(int publisherId)
        {
            var publishersDetails = await _service.GetByIdAsync(publisherId);
            if (publishersDetails == null) return View("Not Found");

            return View(publishersDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("PublisherId, Name,Location,LogoUrl")] Publisher publisher)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(publisher);
            }

            await _service.UpdateAsync(publisher.PublisherId, publisher);
            return RedirectToAction(nameof(Index));
        }

        // GET: Delete Publisher
        public async Task<IActionResult> Delete(int publisherId)
        {
            var publisherDetails = await _service.GetByIdAsync(publisherId);

            // Check if details are null
            if (publisherDetails == null) return View("Not Found");

            return View(publisherDetails);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int publisherId)
        {
            var publisherDetails = await _service.GetByIdAsync(publisherId);

            // Check if details are null
            if (publisherDetails == null) return NotFound();

            await _service.DeleteAsync(publisherId);

            return RedirectToAction(nameof(Index));
        }
    }
}
