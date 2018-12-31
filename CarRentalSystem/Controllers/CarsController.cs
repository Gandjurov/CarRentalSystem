namespace CarRentalSystem.Controllers
{
    using CarRentalSystem.Data;
    using CarRentalSystem.Models.Cars;
    using Microsoft.AspNet.Identity;
    using System.Web.Mvc;

    public class CarsController : Controller
    {
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(CreateCarModel model)
        {
            if (this.ModelState.IsValid)
            {
                var ownerId = this.User.Identity.GetUserId();

                var car = new Car
                {
                    Make = model.Make,
                    Model = model.Model,
                    Color = model.Color,
                    Engine = model.Engine,
                    EngineType = model.EngineType,
                    ImageUrl = model.ImageUrl,
                    Power = model.Power,
                    PricePerDay = model.PricePerDay,
                    Year = model.Year,
                    OwnerId = ownerId
                };

                var db = new CarsDbContext();

                db.Cars.Add(car);

                db.SaveChanges();

                return RedirectToAction("Details", new { id = car.Id });
            }

            return View(model);
        }

    }
}