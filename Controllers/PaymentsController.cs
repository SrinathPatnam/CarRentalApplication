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

namespace CarRentalApplication.Controllers
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly CarContext _context;
        private readonly ApplicationDbContext _applicationDbContext;
        private IHttpContextAccessor _httpContextAccessor;

        public PaymentsController(CarContext context, ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    var carContext_admin = _context.Payment.Include(p => p.Booking);
                    return View(await carContext_admin.ToListAsync());
                }
                else
                {
                    var carContext = _context.Payment.Where(x => x.UserId == User.Identity.GetUserId()).Include(p => p.Booking);
                    return View(await carContext.ToListAsync());
                }
            }
            else
            {
                return RedirectToAction("/Identity/Account/Login");
            }
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Payments/Create
        public IActionResult Create(int? id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var booking_pay = _context.Booking.FindAsync(id).Result;
                var Car_pay = _context.Car.FindAsync(booking_pay.CarId).Result;
                ViewBag.Price_perday = Car_pay.Price;
                Payment payment = new Payment();
                payment.Booking = booking_pay;
                payment.BookingId = booking_pay.BookingId;
                payment.ExpiryDate = DateTime.UtcNow;
                return View(payment);
            }
            else
            {
                return RedirectToAction("/Identity/Account/Login");
            }
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,CardNumber,ExpiryDate,CvvNumber,BookingId,CreatedAt,UpdatedAt")] Payment payment)
        {
            var booking_pay = _context.Booking.FindAsync(payment.BookingId).Result;
            var Car_pay = _context.Car.FindAsync(booking_pay.CarId).Result;
            ViewBag.Price_perday = Car_pay.Price;
            if (ModelState.IsValid)
            {
                payment.UserId = User.Identity.GetUserId();
                payment.Payment_Price = ((booking_pay.ToDate - booking_pay.FromDate).Days) * Car_pay.Price;
                _context.Add(payment);
                //var booking_create = _context.Booking.FindAsync(payment.Id).Result;
                booking_pay.Payment_Status = Payment_Status.Paid;
                _context.Update(booking_pay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "BookingId", "BookingId", payment.BookingId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            ViewData["BookingId"] = new SelectList(_context.Booking, "BookingId", "BookingId", payment.BookingId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,CardNumber,ExpiryDate,CvvNumber,BookingId,CreatedAt,UpdatedAt")] Payment payment)
        {
            if (id != payment.PaymentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    payment.UpdatedAt = DateTime.UtcNow;
                    _context.Update(payment);
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PaymentExists(payment.PaymentId))
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
            ViewData["BookingId"] = new SelectList(_context.Booking, "BookingId", "BookingId", payment.BookingId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Booking)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var payment = await _context.Payment.FindAsync(id);
            _context.Payment.Remove(payment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
            return _context.Payment.Any(e => e.PaymentId == id);
        }
    }
}
