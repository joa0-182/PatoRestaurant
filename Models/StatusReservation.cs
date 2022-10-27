using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatoRestaurant.Models
{
    [Table("StatusReservation")]
    public class StatusReservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o {0}")]
        [StringLength(30, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        public string Name { get; set; }
    }
}