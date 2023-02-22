using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRentalApplication.Data;
using CarRentalApplication.Models;

namespace CarRentalApplication.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CarContext context)
        {
            context.Database.EnsureCreated();

            //Look for any Cars.
            if (context.Car.Any())
                {
                    return;   // DB has been seeded
                }

            var cars = new Car[]
            {
            new Car{Carname="Tesla",Cartype="Sedan",Region = "Brampton", Price = 100},
            new Car{Carname="Acura",Cartype="SUV",Region = "Milton", Price = 80},
            new Car{Carname="Benz",Cartype="Sedan",Region = "OakVille", Price = 120},
            new Car{Carname="Audi",Cartype="SUV",Region = "Mississauga", Price = 150}
            };
            foreach (Car c in cars)
            {
                context.Car.Add(c);
            }
            context.SaveChanges();

            //Look for any Categories.
            if (context.Category.Any())
            {
                return;   // DB has been seeded
            }

            var categories = new Category[]
            {
            new Category{CategoryId=1,Name="Mini",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="Economy",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="Compact",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="Compact State",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="Intermediate",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="Standard",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="Full-Size",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="Luxury",Seats = 2, Bags = 1, BestFor= "City trips"},
            new Category{CategoryId=1,Name="SUV",Seats = 2, Bags = 1, BestFor= "City trips"}            
            };
            foreach (Category t in categories)
            {
                context.Category.Add(t);
            }
            context.SaveChanges();
        }
    }
}