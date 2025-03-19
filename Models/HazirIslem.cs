using System.ComponentModel.DataAnnotations;

namespace TamirhaneMVC.Models
{
    public class HazirIslem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IslemAdi { get; set; }

        [Required]
        [Range(0, 99999)]
        public decimal Fiyat { get; set; }
    }
}
