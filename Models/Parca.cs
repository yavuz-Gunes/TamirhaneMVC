using System.ComponentModel.DataAnnotations;

namespace TamirhaneMVC.Models
{
    public class Parca
    {
        public int Id { get; set; }
        public string ParcaAdi { get; set; }
        
        [Required]
        [Range(0, 99999)]
        public decimal Fiyat { get; set; }
        
        [Required]
        [Range(0, 10000)]
        public int StokMiktari { get; set; } = 0;
    }
}