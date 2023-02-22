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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace CarRentalApplication.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly CarContext _context; 
        private readonly ApplicationDbContext _applicationDbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public BookingsController(CarContext context, ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var carContext_admin = _context.Booking.Include(b => b.Car);
                    return View(await carContext_admin.ToListAsync());
                }
                else
                { 
                var carContext = _context.Booking.Where(x => x.UserId == User.Identity.GetUserId()).Include(b => b.Car);
                return View(await carContext.ToListAsync());
                }
            }
            else
            {
                return RedirectToAction("/Identity/Account/Login");
            }            
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Car)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create/5
        public IActionResult Create(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var Car_create = _context.Car.FindAsync(id).Result;
                Booking booking = new Booking();
                booking.Car = Car_create;
                booking.CarId = Car_create.Carid;
                booking.FromDate = DateTime.UtcNow;
                booking.ToDate = DateTime.UtcNow;
                booking.BookingList = _context.Booking.Where(x => x.CarId == Car_create.Carid).ToList();
                return View(booking);
            }
            else
            {
                return RedirectToAction("/Identity/Account/Login");
            }
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,FromDate,ToDate,ExtraFeatures,CarId")] Booking booking)
        {
            booking.BookingList = _context.Booking.Where(x => x.CarId == booking.CarId).ToList();
            // check if date entered is in past
            if (booking.FromDate < DateTime.Today)
            {
                ViewData["Cars"] = new SelectList(_context.Car, "Carid", "Carname", booking.CarId);
                ModelState.AddModelError(booking.FromDate.ToString(), "Cannot make a booking for past days ");
                booking.Car = _context.Car.FindAsync(booking.CarId).Result;
                return View(booking);
            }

            // validate if to date is after from date 
            if (booking.ToDate < booking.FromDate)
            {
                ViewData["Cars"] = new SelectList(_context.Car, "Carid", "Carname", booking.CarId);
                ModelState.AddModelError(booking.ToDate.ToString(), "To date should after from date");
                booking.Car = _context.Car.FindAsync(booking.CarId).Result;
                return View(booking);
            }
                                          
            var bookings = _context.Booking.Where(x => x.CarId == booking.CarId).ToList();            
            foreach (var book in bookings)
            {
                // validate the input dates with previous bookings of the selected car
                if (book.FromDate < booking.ToDate && booking.FromDate < book.ToDate)
                {
                    ViewData["Cars"] = new SelectList(_context.Car, "Carid", "Carname", booking.CarId);
                    ModelState.AddModelError(booking.ToDate.ToString(), "date not in available range");
                    booking.Car = _context.Car.FindAsync(booking.CarId).Result;
                    return View(booking);
                }
            }

            var Car = _context.Car.FindAsync(booking.CarId).Result;
            if (ModelState.IsValid)
            {
                booking.UserId = User.Identity.GetUserId();
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Cars"] = new SelectList(_context.Car, "Carid", "Carname", booking.CarId);
            booking.Car = Car;
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Car, "Carid", "Carid", booking.CarId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,FromDate,ToDate,ExtraFeatures,CarId,status,CreatedAt,UpdatedAt")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    booking.UpdatedAt = DateTime.UtcNow;
                    booking.UserId = User.Identity.GetUserId();
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            
            ViewData["CarId"] = new SelectList(_context.Car, "Carid", "Carid", booking.CarId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Car)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(booking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }
    }
}
