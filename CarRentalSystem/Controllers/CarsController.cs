namespace CarRentalSystem.Controllers
{
    using CarRentalSystem.Data;
    using CarRentalSystem.Models.Cars;
    using CarRentalSystem.Models.Renting;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    public class CarsController : Controller
    {
        public ActionResult All(int page = 1, string user = null, string search = null)
        {
            var db = new CarsDbContext();

            var pageSize = 5;

            var carsQuery = db.Cars.AsQueryable();

            if (search != null)
            {
                carsQuery = carsQuery
                            .Where(c => c.Make.ToLower().Contains(search.ToLower()) ||
                                   c.Model.ToLower().Contains(search.ToLower()));
            }

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
                    Id = c.Id,
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
                    TotalRents = c.Rentings.Count(),
                    ContactInformation = c.Owner.Email
                })
                .FirstOrDefault();

            if (car == null)
            {
                return HttpNotFound();
            }
            
            return View(car);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Rent(RentCarModel rentCarModel)
        {
            if (rentCarModel.Days < 1 || rentCarModel.Days > 10)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var db = new CarsDbContext();

            var car = db.Cars
                        .Where(c => c.Id == rentCarModel.CarId)
                        .Select(c => new
                        {
                            c.IsRented,
                            c.PricePerDay,
                            c.ImageUrl,
                            FullName = c.Make + " " + c.Model + " (" + c.Year + ")"
                        })
                        .FirstOrDefault();

            if (car == null || car.IsRented)
            {
                return HttpNotFound();
            }

            rentCarModel.CarName = car.FullName;
            rentCarModel.CarImageUrl = car.ImageUrl;
            rentCarModel.PricePerDay = car.PricePerDay;
            rentCarModel.TotalPrice = car.PricePerDay * rentCarModel.Days;


            return View(rentCarModel);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Rent(int carId, int days)
        {
            var db = new CarsDbContext();

            var car = db.Cars
                        .Where(c => c.Id == carId)
                        .FirstOrDefault();

            var userId = this.User.Identity.GetUserId();

            if (car == null || car.IsRented || car.OwnerId == userId)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var renting = new Renting
            {
                CarId = carId,
                Days = days,
                RentedOn = DateTime.Now, //if this application will be global, them must be DateTime.UtcNow
                UserId = userId,
                TotalPrice = days * car.PricePerDay
            };

            car.IsRented = true;

            db.Rentings.Add(renting);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = car.Id});
        }


        [Authorize]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new CarsDbContext())
            {
                var car = db.Cars
                            .Where(c => c.Id == id)
                            .FirstOrDefault();

                if (car == null || !IsAuthorized(car))
                {
                    return HttpNotFound();
                }

                var model = new CarViewModel();

                model.Make = car.Make;
                model.Model = car.Model;
                model.Color = car.Color;
                model.Engine = car.Engine;
                model.EngineType = car.EngineType;
                model.ImageUrl = car.ImageUrl;
                model.Power = car.Power;
                model.PricePerDay = car.PricePerDay;
                model.Year = car.Year;
                
                
                return View(model);
            }
            
        }

        [Authorize]
        //[ActionName("Edit")]
        [HttpPost]
        public ActionResult Edit(CarViewModel carModel)
        {
            if (carModel != null && this.ModelState.IsValid)
            {
                var ownerId = this.User.Identity.GetUserId();

                using (var db = new CarsDbContext())
                {
                    var car = db.Cars.Find(carModel.Id);

                    if (car == null || !IsAuthorized(car))
                    {
                        return HttpNotFound();
                    }

                    car.Id = carModel.Id;
                    car.Make = carModel.Make;
                    car.Model = carModel.Model;
                    car.Color = carModel.Color;
                    car.Engine = carModel.Engine;
                    car.EngineType = carModel.EngineType;
                    car.ImageUrl = carModel.ImageUrl;
                    car.Power = carModel.Power;
                    car.PricePerDay = carModel.PricePerDay;
                    car.Year = carModel.Year;
                    car.OwnerId = ownerId;

                    db.SaveChanges();
                }
                

                return RedirectToAction("Details", new { id = carModel.Id });
            }

            return View(carModel);
        }
        

        [Authorize]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (var db = new CarsDbContext())
            {
                var car = db.Cars
                            .Where(c => c.Id == id)
                            .FirstOrDefault();

                if (car == null || !IsAuthorized(car))
                {
                    return HttpNotFound();
                }
                

                return View();
            }

        }

        [Authorize]
        [ActionName("Delete")]
        [HttpPost]
        public ActionResult ConfirmDeleted(int id)
        {
            using (var db = new CarsDbContext())
            {
                var car = db.Cars
                            .Where(c => c.Id == id)
                            .FirstOrDefault();

                if (car == null || !IsAuthorized(car))
                {
                    return HttpNotFound();
                }

                db.Cars.Remove(car);
                db.SaveChanges();
            }

            return RedirectToAction("All");
        }

        [Authorize]
        [HttpPost]
        public ActionResult Cancel(int id)
        {
            
            var db = new CarsDbContext();

            var car = db.Cars
                        .Where(c => c.Id == id)
                        .Select(c => new
                        {
                            c.Id
                        })
                        .FirstOrDefault();

            if (car == null)
            {
                return HttpNotFound();
            }


            return RedirectToAction("Details", new { id = car.Id });
        }

        private bool IsAuthorized(Car car)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var isOwner = car.IsOwner(this.User.Identity.GetUserId());

            return isAdmin || isOwner;
        }
    }
}