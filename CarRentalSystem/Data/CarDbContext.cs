namespace CarRentalSystem.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class CarsDbContext : IdentityDbContext<User>
    {
        public CarsDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static CarsDbContext Create()
        {
            return new CarsDbContext();
        }
    }
}