using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TamirhaneMVC.Models
{
    public class IslemDetay
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Islem")]
        public int IslemId { get; set; }
        public Islem Islem { get; set; }

        [ForeignKey("HazirIslem")]
        public int HazirIslemId { get; set; }
        public HazirIslem HazirIslem { get; set; }

        public string IslemAdi { get; set; }
        public decimal Fiyat { get; set; }
    }
} 