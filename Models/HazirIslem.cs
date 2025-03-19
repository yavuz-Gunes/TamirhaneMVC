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
        public decimal Fiyat { get; set; }
    }
}
