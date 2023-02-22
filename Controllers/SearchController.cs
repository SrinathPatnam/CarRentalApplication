using CarRentalApplication.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalApplication.Controllers
{
    public class SearchController : Controller
    {
        private readonly CarContext _context;

        public SearchController(CarContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Recommend()
        {
            FormCollection nvc = (FormCollection)Request.Form;
            var seats = Int32.Parse(nvc["seats"]);
            var bags = Int32.Parse(nvc["bags"]);
            var budget = Int32.Parse(nvc["budget"]);
            var bestFor = nvc["bestFor"].ToString();
            
            var cars = _context.Car.Where(t => t.Category.Seats >= seats)
                    .Where(t => t.Price <= budget)
                    .Where(t => t.Category.Bags <= bags)
                    .Where(t => bestFor.Contains(t.Category.Name))
                    .OrderByDescending(t=>t.Rating)
                .Include(b => b.Category);

            ViewData["recommendList"] = cars.OrderByDescending(s => s.Rating).Take(3);

            return View(await cars.ToListAsync());
        }
    }
}
