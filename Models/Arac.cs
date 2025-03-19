using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TamirhaneMVC.Models
{
    public class Arac
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Plaka { get; set; }

        public DateTime GirişTarihi { get; set; } = DateTime.Now;

        public List<Islem> Islemler { get; set; } = new List<Islem>();
    }
}
