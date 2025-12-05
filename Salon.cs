using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace sedatodev1.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }

        [ValidateNever]
        public ICollection<Hizmet> Hizmetler { get; set; }

        [ValidateNever]
        public ICollection<Antrenor> Antrenorler { get; set; }
    }
}