using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace sedatodev1.Models
{
    public class Antrenor
    {
        public int Id { get; set; }
        public int SalonId { get; set; }

        [ValidateNever]
        public Salon Salon { get; set; }
        public string Telefon { get; set; }

        public string Ad { get; set; }
        public string Uzmanlik { get; set; } 

        [ValidateNever]
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }
        [ValidateNever]
        public ICollection<AntrenorMusaitlik> Musaitliklar { get; set; }
        [ValidateNever]
        public ICollection<Randevu> Randevular { get; set; }
    }
}
