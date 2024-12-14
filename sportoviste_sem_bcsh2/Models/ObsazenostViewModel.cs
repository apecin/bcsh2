using Microsoft.AspNetCore.Mvc;

namespace sportoviste_sem_bcsh2.Models
{
    public class ObsazenostViewModel
    {
        public string Hriste { get; set; } = string.Empty;
        public List<Rezervace> Rezervace { get; set; } = new List<Rezervace>();
        public TimeSpan OtevrenoOd { get; set; }
        public TimeSpan OtevrenoDo { get; set; }
    }

}
