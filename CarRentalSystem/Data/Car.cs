using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalSystem.Data
{
    public class Car
    {
        public int Id { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public int Year { get; set; }

        public string Color { get; set; }

        public double Engine { get; set; }

        public EngineType EngineType { get; set; }

        public int Power { get; set; }

        //In BGN
        public decimal PricePerDay { get; set; }

        public string ImageUrl { get; set; }

        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }
    }
}