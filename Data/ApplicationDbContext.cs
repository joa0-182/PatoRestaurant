using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PatoRestaurant.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
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

        #region Seed Roles
        List<IdentityRole> listRoles = new()
        {
            new IdentityRole{
                Id = Guid.NewGuid().ToString(),
                Name = "Administrador",
                NormalizedName = "ADMINISTRADOR"
            },
            new IdentityRole{
                Id = Guid.NewGuid().ToString(),
                Name = "Usuário",
                NormalizedName = "USUÁRIO"
            }
        };
        builder.Entity<IdentityRole>().HasData(listRoles);
        #endregion

        #region Seed ApplicationUser - Admin
        var userId = Guid.NewGuid().ToString();
        var hash = new PasswordHasher<ApplicationUser>();
        builder.Entity<ApplicationUser>().HasData(
            new ApplicationUser{
                Id = userId,
                Name = "José Antonio Gallo Junior",
                UserName = "admin@pato.com",
                NormalizedUserName = "ADMIN@PATO.COM",
                Email = "admin@pato.com",
                NormalizedEmail = "ADMIN@PATO.COM",
                EmailConfirmed = true,
                PasswordHash = hash.HashPassword(null, "123456"),
                SecurityStamp = hash.GetHashCode().ToString(),
                ProfilePicture = @"\img\avatar.png"
            }
        );

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>{
                UserId = userId,
                RoleId = listRoles[0].Id
            }
        );
        #endregion

        #region Seed StatusReservation
        List<StatusReservation> listStatusReservation = new()
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
        builder.Entity<StatusReservation>()
            .HasData(listStatusReservation);
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
                Name = "Sobremesas"
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
