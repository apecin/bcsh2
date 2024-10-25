namespace sportoviste_sem_bcsh2.Models
{
    public class Rezervace
    {
        public int Id { get; set; }
        public DateTime DatumOd { get; set; }
        public DateTime DatumDo { get; set; }
        public int HristeId { get; set; }
        public Hriste Hriste { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
    }
}
