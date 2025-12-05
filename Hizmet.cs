using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace sedatodev1.Models
{
    public class Hizmet
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; }

        public string Ad { get; set; }
        public int Sure { get; set; }     
        public decimal Ucret { get; set; }

        [ValidateNever]
        public ICollection<AntrenorHizmet> AntrenorHizmetler { get; set; }
        [ValidateNever]
        public ICollection<Randevu> Randevular { get; set; }
    }
}
