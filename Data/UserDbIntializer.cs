using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CarRentalApplication.Utility.enumManager;

namespace CarRentalApplication.Data
{
    public static class UserDbIntializer
    {
        //private readonly ApplicationDbContext _db;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        //public UserDbIntializer(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        //{
        //    _db = db;
        //    _userManager = userManager;
        //    _roleManager = roleManager;
        //}

        public static void initializer(ApplicationDbContext _db, UserManager<IdentityUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch { }


            //If Admin in not exists
            if (!_db.Roles.Any(r => r.Name == eRoles.Admin.ToString()))
            {

                _roleManager.CreateAsync(new IdentityRole { Name = eRoles.Admin.ToString() }).GetAwaiter().GetResult();
            }

            //If customer
            if (!_db.Roles.Any(r => r.Name == eRoles.Customer.ToString()))
            {
                _roleManager.CreateAsync(new IdentityRole { Name = eRoles.Customer.ToString() }).GetAwaiter().GetResult();
            }

            var user = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };
            _userManager.CreateAsync(user, "Admin@123").GetAwaiter().GetResult();

            //var user = _db.ApplicationUser.Where(u => u.Email == "admin@gmail.com").FirstOrDefault();
            ////var user = _unitOfWork.ApplicationUserRepository.GetFirstOrDefault(x => x.Email == "admin@gmail.com").GetAwaiter().GetResult();
            //if (user != null)
            //{
            //    _db.Entry(user).State = EntityState.Detached;
            //}
            _userManager.AddToRoleAsync(user, eRoles.Admin.ToString()).GetAwaiter().GetResult();

        }
    }
}
