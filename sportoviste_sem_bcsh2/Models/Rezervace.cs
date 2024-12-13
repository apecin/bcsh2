namespace sportoviste_sem_bcsh2.Models
{
    public class Rezervace
    {
        public int Id { get; set; }
        public int HristeId { get; set; }
        public Hriste? Hriste { get; set; }
        public string Klient { get; set; } = string.Empty; 
        public DateTime Cas { get; set; }


        public bool IsValid()
        {
            return Cas.TimeOfDay >= Hriste.OtevrenoOd && Cas.TimeOfDay <= Hriste.OtevrenoDo;
        }

    }
}
