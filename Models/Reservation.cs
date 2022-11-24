using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatoRestaurant.Models
{
    [Table("Reservation")]
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt16 Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe seu {0}")]
        [StringLength(60, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Data da Reserva")]
        [Required(ErrorMessage = "Informe a {0}")]
        public DateTime ReservationDate { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "Informe seu {0}")]
        [StringLength(20, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        public string Phone { get; set; }

        [Display(Name = "Convidados")]
        [Required(ErrorMessage = "Informe o número de convidados")]
        public byte Guests { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(100, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        public string Email { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Status da Reserva")]
        public byte StatusReservationId { get; set; }
        [ForeignKey("StatusReservationId")]
        public StatusReservation StatusReservation { get; set; }
    }
}