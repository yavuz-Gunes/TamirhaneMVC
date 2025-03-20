using System;
using System.Collections.Generic;
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
        
        public DateTime IslemTarihi { get; set; } = DateTime.Now;
        
        public string IslemDetayi { get; set; }

        [ForeignKey("Arac")]
        public int AracId { get; set; }
        public Arac Arac { get; set; }
        
        public List<IslemParca> KullanilanParcalar { get; set; } = new List<IslemParca>();
    }
}
