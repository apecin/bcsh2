namespace sportoviste_sem_bcsh2.Models
{
    public class Hriste
    {
        public int Id { get; set; }
        public string? Nazev { get; set; }
        public string? Typ { get; set; }
        public int SportovisteId { get; set; }
        public Sportoviste? Sportoviste { get; set; }
        public List<Rezervace>? Rezervace { get; set; }
    }
}
