using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarRentalApplication.Data;
using CarRentalApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace CarRentalApplication.Controllers
{        
    public class CarsController : Controller
    {
        private readonly CarContext _context;
        private IHostingEnvironment Environment;

        public CarsController(CarContext context, IHostingEnvironment _environment)
        {
            _context = context;
            Environment = _environment;
        }
        // GET: Cars
        public async Task<IActionResult> Index(
            string currentFilterLocation,
            string searchLocation,
            int? pageNumber)
        {
            var cars = from c in _context.Car select c;

            if (searchLocation != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchLocation = currentFilterLocation;
            }

            ViewData["currentFilterLocation"] = searchLocation;

            if (!String.IsNullOrEmpty(searchLocation))
            {
                cars = cars.Where(s => s.Region.Contains(searchLocation));
            }

            int pageSize = 10;
            ViewData["Categories"] = new SelectList(_context.Category, "CategoryId", "Name");
            return View(await PaginatedList<CarRentalApplication.Models.Car>.CreateAsync(cars.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Cars/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Carid == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [Authorize]
        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Category, "CategoryId", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Carid,Carname,Cartype,Region,Price,CategoryId,ImageFile")] Car car)
        {
            if (ModelState.IsValid)
            {                
                if(car.ImageFile != null)
                {
                    string wwwPath = this.Environment.WebRootPath;
                    string contentPath = this.Environment.ContentRootPath;

                    string path = Path.Combine(this.Environment.WebRootPath, "img");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    IFormFile ImageFile = car.ImageFile;

                    string fileName = Path.GetFileName(ImageFile.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    car.Image = fileName;
                }

                var Category_car = _context.Category.FindAsync(car.CategoryId).Result;
                car.Cartype = Category_car.Name;
                car.Rating = 4;
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        [Authorize]
        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Carid,Carname,Cartype,Region,Price,status")] Car car)
        {
            if (id != car.Carid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Carid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

        [Authorize]
        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Car
                .FirstOrDefaultAsync(m => m.Carid == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Car.FindAsync(id);
            _context.Car.Remove(car);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.Carid == id);
        }
    }
}
