namespace sedatodev1.Models
{
    public class Randevu
    {
        public int Id { get; set; }

        public int HizmetId { get; set; }
        public Hizmet Hizmet { get; set; }

        public int AntrenorId { get; set; }
        public Antrenor Antrenor { get; set; }

        public string UyeId { get; set; }
        public Uye Uye { get; set; }

        public DateTime Tarih { get; set; }
        public TimeSpan Saat { get; set; }   
    }
}
