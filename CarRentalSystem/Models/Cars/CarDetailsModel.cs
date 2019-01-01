namespace CarRentalSystem.Models.Cars
{
    using CarRentalSystem.Data;

    public class CarDetailsModel
    {
        public string Make { get; set; }
        
        public string Model { get; set; }
        
        public int Year { get; set; }

        public string Color { get; set; }

        public double Engine { get; set; }

        public EngineType EngineType { get; set; }

        public int? Power { get; set; }
        
        public decimal PricePerDay { get; set; }
        
        public string ImageUrl { get; set; }

        public string ContactInformation { get; set; }
    }
}