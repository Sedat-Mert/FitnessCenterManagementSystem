using System.ComponentModel.DataAnnotations;

namespace sedatodev1.Models
{
    public class AiPlanRequestModel
    {
        [Required(ErrorMessage = "Boy zorunludur")]
        [Range(100, 250, ErrorMessage = "Boy 100 ile 250 cm arasında olmalıdır")]
        public int Boy { get; set; }

        [Required(ErrorMessage = "Kilo zorunludur")]
        [Range(30, 300, ErrorMessage = "Kilo 30 ile 200 kg arasında olmalıdır")]
        public int Kilo { get; set; }

        [Required(ErrorMessage = "Lütfen Vücut tipi seçiniz")]
        public string VucutTipi { get; set; }

        [Required(ErrorMessage = "Lütfen Hedef seçiniz")]
        public string Hedef { get; set; }
    }
}
