using System.ComponentModel.DataAnnotations;

namespace sportoviste_sem_bcsh2.Models
{
    public class Sportoviste
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Název je povinný.")]
        public string Nazev { get; set; }

        // Inicializace prázdným seznamem, aby byl model vždy validní
        public List<Hriste> Hriste { get; set; } = new List<Hriste>();
    }

}
