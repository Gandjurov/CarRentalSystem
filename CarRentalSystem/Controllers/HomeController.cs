using CarRentalSystem.Data;
using CarRentalSystem.Models.Cars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var db = new CarsDbContext();

            var cars = db.Cars
                .OrderByDescending(c => c.Id)
                .Take(3)
                .Select(c => new HomeIndexCarModel
                {
                    Id = c.Id,
                    ImageUrl = c.ImageUrl,
                    Make = c.Make,
                    Model = c.Model,
                    Year = c.Year
                })
                .ToList();

            return View(cars);
        }
    }
}