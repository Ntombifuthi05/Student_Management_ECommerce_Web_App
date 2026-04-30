//Group NAme: Sharp Future
//223001495 MAKOSONKE NP
//223074090 LIPHAPANG S
//223050534 BLOU A
//223056856 RAMMUTLA OR
//223089666 Pico MNP
//223058521 SHUPING KO

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPNETCore_DB.Data;
using ASPNETCore_DB.Models;

namespace ASPNETCore_DB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ConsumerController : Controller
    {
        private readonly SQLiteDBContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ConsumerController(SQLiteDBContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var consumers = from c in _context.Consumers select c;

            if (!string.IsNullOrEmpty(searchString))
            {
                consumers = consumers.Where(c => c.Name.Contains(searchString));
            }

            return View(await consumers.ToListAsync());
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Consumer consumer, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string folder = "images/";
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string path = Path.Combine(_hostEnvironment.WebRootPath, folder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    consumer.ImagePath = "/" + folder + fileName;
                }

                _context.Add(consumer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(consumer);
        }

       // [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var consumer = await _context.Consumers.FindAsync(id);
            return consumer == null ? NotFound() : View(consumer);
        }

        //[Authorize(Roles = "Consumer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Consumer consumer, IFormFile imageFile)
        {
            if (id != consumer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string folder = "images/";
                    string fileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string path = Path.Combine(_hostEnvironment.WebRootPath, folder, fileName);

                    using var stream = new FileStream(path, FileMode.Create);
                    await imageFile.CopyToAsync(stream);
                    consumer.ImagePath = "/" + folder + fileName;
                }

                _context.Update(consumer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(consumer);
        }


        //[Authorize(Roles = "Consumer")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var consumer = await _context.Consumers.FirstOrDefaultAsync(m => m.Id == id);
            return consumer == null ? NotFound() : View(consumer);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var consumer = await _context.Consumers.FindAsync(id);
            return consumer == null ? NotFound() : View(consumer);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consumer = await _context.Consumers.FindAsync(id);
            if (consumer != null)
            {
                _context.Consumers.Remove(consumer);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

