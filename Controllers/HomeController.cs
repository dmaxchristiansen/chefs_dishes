using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChefsDishes.Models;
using Microsoft.EntityFrameworkCore;

namespace ChefsDishes.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            ViewBag.AllChefs = dbContext.Chefs.Include(c => c.CreatedDishes);
            return View();
        }

        [HttpGet]
        public IActionResult NewChef()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateChef(Chef newChef)
        {
            if (ModelState.IsValid)
            {
                DateTime today = DateTime.Now;
                DateTime dob = newChef.DateOfBirth;
                int age = today.Year - dob.Year;
                newChef.Age = age;
                dbContext.Chefs.Add(newChef);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("NewChef");
        }

        [HttpGet]
        public IActionResult Dishes()
        {
            ViewBag.AllDishes = dbContext.Dishes.Include(d => d.Chef);
            return View();
        }

        [HttpGet]
        public IActionResult NewDish()
        {
            ViewBag.AllChefs = dbContext.Chefs;
            return View();
        }

        [HttpPost]
        public IActionResult CreateDish(Dish newDish)
        {
            if (ModelState.IsValid)
            {
                dbContext.Dishes.Add(newDish);
                dbContext.SaveChanges();
                return RedirectToAction("Dishes");
            }
            ViewBag.AllChefs = dbContext.Chefs;
            return View("NewDish");
        }
    }
}