using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PatoRestaurant.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet <ApplicationUser> ApplicationUsers { get; set; }

    public DbSet <Category> Categories { get; set; }

    public DbSet <Product> Products { get; set; }

    public DbSet <Reservation> Reservations { get; set; }

    public DbSet <Review> Reviews { get; set; }

    public DbSet <SocialEvent> SocialEvents { get; set; }

    public DbSet <StatusReservation> StatusReservations { get; set; }
}
