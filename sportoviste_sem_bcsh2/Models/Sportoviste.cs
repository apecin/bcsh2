namespace sportoviste_sem_bcsh2.Models
{
    public class Sportoviste
    {
        public int Id { get; set; }
        public string Nazev { get; set; }
        public List<Hriste> Hriste { get; set; }
    }
}
