using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TamirhaneMVC.Models
{
    public class Islem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IslemAdi { get; set; }

        [Required]
        public decimal Fiyat { get; set; }

        [ForeignKey("Arac")]
        public int AracId { get; set; }
        public Arac Arac { get; set; }
    }
}
