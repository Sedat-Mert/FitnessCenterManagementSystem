using Microsoft.AspNetCore.Identity;
namespace sedatodev1.Models
    
{
    public class Uye : IdentityUser
    {
        public string AdSoyad { get; set; }

        public ICollection<Randevu> Randevular { get; set; }
    }
}