using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PatoRestaurant.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<SocialEvent> SocialEvents { get; set; }
    public DbSet<StatusReservation> StatusReservations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        #region Seed StatusReservation
        List<StatusReservation>listStatusReservation = new () 
        {
            new StatusReservation() 
            {
                Id = 1,
                Name = "Aguardando Confirmação"
            },
            new StatusReservation() 
            {
                Id = 2,
                Name = "Reserva Confirmada"
            },
            new StatusReservation() 
            {
                Id = 3,
                Name = "Reserva Cancelada"
            },
            new StatusReservation() 
            {
                Id = 4,
                Name = "Reserva Reagendada"
            }                       
        };
        builder.Entity<StatusReservation>().HasData(listStatusReservation);
        #endregion

        #region Seed Category
        List<Category> listCategory = new List<Category>()
        {
            new Category()
            {
                Id = 1,
                Name = "Entradas"
            },

            new Category()
            {
                Id = 2,
                Name = "Principal"
            },
            new Category()
            {
                Id = 3,
                Name = "Bebidas"
            },
            new Category()
            {
                Id = 4,
                Name = "Sobremesa"
            },
            new Category()
            {
                Id = 5,
                Name = "Almoço"
            },
            new Category()
            {
                Id = 6,
                Name = "Jantar"
            }
        };
        builder.Entity<Category>().HasData(listCategory);   
        #endregion
    }

}
