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

        // Otevírací doba
        public TimeSpan OtevrenoOd { get; set; } = new TimeSpan(8, 0, 0); // Defaultně od 8:00
        public TimeSpan OtevrenoDo { get; set; } = new TimeSpan(20, 0, 0); // Defaultně do 20:00
    }
}
