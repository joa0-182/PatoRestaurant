using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PatoRestaurant.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(60)]
        public string Name { get; set; }

        [StringLength(400)]
        public string ProfilePicture { get; set; }
    }
}