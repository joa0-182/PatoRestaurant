using System.ComponentModel.DataAnnotations;

namespace PatoRestaurant.ViewModels
{
    public class ReservationVM
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Informe o seu {0}")]
        [StringLength(60, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Data da Reserva")]
        [Required(ErrorMessage = "Informe a {0}")]
        public string ReservationDate { get; set; }

        [Display(Name = "Hora da Reserva")]
        [Required(ErrorMessage = "Informe a {0}")]
        public string ReservationTime { get; set; }

        [Display(Name = "Celular")]
        [Required(ErrorMessage = "Informe o seu {0}")]
        [StringLength(20, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        public string Phone { get; set; }

        [Display(Name = "Convidados")]
        [Required(ErrorMessage = "Informe o número de convidados")]
        public byte Guests { get; set; }

        [Display(Name = "E-mail")]
        [StringLength(100, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido!")]
        public string Email { get; set; }
    }
}