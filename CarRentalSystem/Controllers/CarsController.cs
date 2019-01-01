namespace CarRentalSystem.Controllers
{
    using CarRentalSystem.Data;
    using CarRentalSystem.Models.Cars;
    using Microsoft.AspNet.Identity;
    using System.Linq;
    using System.Web.Mvc;

    public class CarsController : Controller
    {
        public ActionResult All(int page = 1, string user = null)
        {
            var db = new CarsDbContext();

            var pageSize = 5;

            var carsQuery = db.Cars.AsQueryable();

            if (user != null)
            {
                carsQuery = carsQuery
                    .Where(c => c.Owner.Email == user);
            }

            var cars = carsQuery
                        .OrderByDescending(c => c.Id)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Select(c => new CarListingModel
                        {
                            Id = c.Id,
                            ImageUrl = c.ImageUrl,
                            Make = c.Make,
                            Model = c.Model,
                            Year = c.Year,
                            PricePerDay = c.PricePerDay,
                            IsRented = c.IsRented
                        })
                        .ToList();

            ViewBag.CurrentPage = page;

            return View(cars);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateCarModel carModel)
        {
            if (carModel != null && this.ModelState.IsValid)
            {
                var ownerId = this.User.Identity.GetUserId();

                var car = new Car
                {
                    Make = carModel.Make,
                    Model = carModel.Model,
                    Color = carModel.Color,
                    Engine = carModel.Engine,
                    EngineType = carModel.EngineType,
                    ImageUrl = carModel.ImageUrl,
                    Power = carModel.Power,
                    PricePerDay = carModel.PricePerDay,
                    Year = carModel.Year,
                    OwnerId = ownerId
                };

                var db = new CarsDbContext();

                db.Cars.Add(car);

                db.SaveChanges();

                return RedirectToAction("Details", new { id = car.Id });
            }

            return View(carModel);
        }

        public ActionResult Details(int id)
        {
            var db = new CarsDbContext();

            var car = db.Cars
                .Where(c => c.Id == id)
                .Select(c => new CarDetailsModel
                {
                    Color = c.Color,
                    Engine = c.Engine,
                    EngineType = c.EngineType,
                    ImageUrl = c.ImageUrl,
                    Make = c.Make,
                    Model = c.Model,
                    Power = c.Power,
                    PricePerDay = c.PricePerDay,
                    Year = c.Year,
                    IsRented = c.IsRented,
                    ContactInformation = c.Owner.Email
                })
                .FirstOrDefault();

            if (car == null)
            {
                return HttpNotFound();
            }
            
            return View(car);
        }
    }
}