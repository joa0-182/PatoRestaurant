using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PatoRestaurant.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public UInt16 Id { get; set; }

        [Display(Name = "Nome do Avaliador")]
        [Required(ErrorMessage = "Informe o {0}")]
        [StringLength(60, ErrorMessage = "O {0} deve possuir no máximo {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Avaliação")]
        [StringLength(2000, ErrorMessage = "A {0} deve possuir no máximo {1} caracteres")]
        public string ReviewText { get; set; }

        [Display(Name = "Data da Avaliação")]
        public DateTime ReviewDate { get; set; } = DateTime.Now;

        [Display(Name = "Imagem")]
        [StringLength(400)]
        public string Image { get; set; }

        [Display(Name = "Nota")]
        [Required(ErrorMessage = "Informe a {0}")]
        public byte Rating { get; set; }
    }
}