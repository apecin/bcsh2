namespace sportoviste_sem_bcsh2.Models
{
    public class Uzivatel
    {
        public int Id { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Heslo { get; set; }
        public string Role { get; set; }
    }
}
