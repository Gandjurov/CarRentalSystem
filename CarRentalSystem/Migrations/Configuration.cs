namespace CarRentalSystem.Migrations
{
    using CarRentalSystem.Data;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<CarRentalSystem.Data.CarsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            ContextKey = "CarRentalSystem.Data.CarsDbContext";
        }

        protected override void Seed(CarsDbContext context)
        {
            if (context.Cars.Any())
            {
                return;
            }

            var user = context.Users.FirstOrDefault();

            if (user == null)
            {
                return;
            }

            context.Cars.Add(new Car
            {
                Make = "BMW",
                Model = "650i",
                Color = "Black",
                Engine = 5.0,
                EngineType = EngineType.Gasoline,
                ImageUrl = "https://i.pinimg.com/originals/d8/3a/63/d83a636b0005231d920b73af45035699.jpg",
                Power = 500,
                PricePerDay = 5000,
                Year = 2016,
                OwnerId = user.Id
            });

            context.Cars.Add(new Car
            {
                Make = "BMW",
                Model = "640i",
                Color = "White",
                Engine = 4.0,
                EngineType = EngineType.Gasoline,
                ImageUrl = "https://i.ytimg.com/vi/_8dThdf6Uyc/maxresdefault.jpg",
                Power = 350,
                PricePerDay = 3000,
                Year = 2017,
                OwnerId = user.Id
            });

            context.Cars.Add(new Car
            {
                Make = "Mercedes",
                Model = "S550",
                Color = "Brown",
                Engine = 5.0,
                EngineType = EngineType.Gasoline,
                ImageUrl = "https://farm9.static.flickr.com/8477/29362532954_11a5ebec7b_b.jpg",
                Power = 520,
                PricePerDay = 5000,
                Year = 2016,
                OwnerId = user.Id
            });

            context.Cars.Add(new Car
            {
                Make = "Mercedes",
                Model = "S550",
                Color = "White",
                Engine = 3.0,
                EngineType = EngineType.Diesel,
                ImageUrl = "https://wrapbullys.com/wp-content/uploads/2016/08/Mercedes-Benz-S550-Satin-White-Wrap-4.jpg",
                Power = 300,
                PricePerDay = 500,
                Year = 2014,
                OwnerId = user.Id
            });

            context.Cars.Add(new Car
            {
                Make = "BMW",
                Model = "760i",
                Color = "White",
                Engine = 6.0,
                EngineType = EngineType.Gasoline,
                ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQxXFNaiclrgz_VdNnfgaUwoV2N5HmyGw1orntUqxngPGAVRsDkrA",
                Power = 660,
                PricePerDay = 6000,
                Year = 2018,
                OwnerId = user.Id
            });

            context.Cars.Add(new Car
            {
                Make = "BMW",
                Model = "740d",
                Color = "Black",
                Engine = 4.0,
                EngineType = EngineType.Diesel,
                ImageUrl = "http://i754.photobucket.com/albums/xx185/sploaterboi/Ace/20170831_142221_zpsofb4ntah.jpg",
                Power = 450,
                PricePerDay = 3500,
                Year = 2016,
                OwnerId = user.Id
            });
        }
    }
}
